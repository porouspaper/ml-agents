import pytest
import torch

from mlagents.trainers.torch.distributions import (
    GaussianDistribution,
    MultiCategoricalDistribution,
    GaussianDistInstance,
    TanhGaussianDistInstance,
    CategoricalDistInstance,
)


@pytest.mark.parametrize("tanh_squash", [True, False])
@pytest.mark.parametrize("conditional_sigma", [True, False])
def test_gaussian_distribution(conditional_sigma, tanh_squash):
    hidden_size = 16
    act_size = 4
    sample_embedding = torch.ones((1, 16))
    gauss_dist = GaussianDistribution(
        hidden_size,
        act_size,
        conditional_sigma=conditional_sigma,
        tanh_squash=tanh_squash,
    )

    # Make sure backprop works
    force_action = torch.zeros((1, act_size))
    optimizer = torch.optim.Adam(gauss_dist.parameters(), lr=3e-3)

    for _ in range(50):
        dist_inst = gauss_dist(sample_embedding)[0]
        if tanh_squash:
            assert isinstance(dist_inst, TanhGaussianDistInstance)
        else:
            assert isinstance(dist_inst, GaussianDistInstance)
        log_prob = dist_inst.log_prob(force_action)
        loss = torch.nn.functional.mse_loss(log_prob, -2 * torch.ones(log_prob.shape))
        optimizer.zero_grad()
        loss.backward()
        optimizer.step()
    for prob in log_prob.flatten():
        assert prob == pytest.approx(-2, abs=0.1)


def test_multi_categorical_distribution():
    hidden_size = 16
    act_size = [3, 3, 4]
    sample_embedding = torch.ones((1, 16))
    gauss_dist = MultiCategoricalDistribution(hidden_size, act_size)

    # Make sure backprop works
    optimizer = torch.optim.Adam(gauss_dist.parameters(), lr=3e-3)

    for _ in range(50):
        dist_insts = gauss_dist(sample_embedding, masks=torch.ones((1, sum(act_size))))
        loss = 0
        for i, dist_inst in enumerate(dist_insts):
            assert isinstance(dist_inst, CategoricalDistInstance)
            log_prob = dist_inst.all_log_prob()
            test_prob = torch.tensor(
                [1.0 - 0.01 * (act_size[i] - 1)] + [0.01] * (act_size[i] - 1)
            )  # High prob for first action
            test_log_prob = test_prob.log()
            loss += torch.nn.functional.mse_loss(log_prob, test_log_prob)
        optimizer.zero_grad()
        loss.backward()
        optimizer.step()
    for dist_inst in dist_insts:
        assert dist_inst.all_log_prob().flatten()[0] == pytest.approx(0, abs=0.1)

    # Test masks
    masks = []
    for branch in act_size:
        masks += [0] * (branch - 1) + [1]
    masks = torch.tensor([masks])
    dist_insts = gauss_dist(sample_embedding, masks=masks)
    for dist_inst in dist_insts:
        log_prob = dist_inst.all_log_prob()
        assert log_prob.flatten()[-1] == pytest.approx(0, abs=0.001)


def test_gaussian_dist_instance():
    act_size = 4
    dist_instance = GaussianDistInstance(
        torch.zeros(1, act_size), torch.ones(1, act_size)
    )
    action = dist_instance.sample()
    assert action.shape == (1, act_size)
    for log_prob in dist_instance.log_prob(torch.zeros((1, act_size))).flatten():
        # Log prob of standard normal at 0
        assert log_prob == pytest.approx(-0.919, abs=0.01)

    for ent in dist_instance.entropy().flatten():
        # entropy of standard normal at 0
        assert ent == pytest.approx(2.83, abs=0.01)


def test_tanh_gaussian_dist_instance():
    act_size = 4
    dist_instance = GaussianDistInstance(
        torch.zeros(1, act_size), torch.ones(1, act_size)
    )
    for _ in range(10):
        action = dist_instance.sample()
        assert action.shape == (1, act_size)
        assert torch.max(action) < 1.0 and torch.min(action) > -1.0


def test_categorical_dist_instance():
    act_size = 4
    test_prob = torch.tensor(
        [1.0 - 0.1 * (act_size - 1)] + [0.1] * (act_size - 1)
    )  # High prob for first action
    dist_instance = CategoricalDistInstance(test_prob)

    for _ in range(10):
        action = dist_instance.sample()
        assert action.shape == (1,)
        assert action < act_size

    # Make sure log_prob of 1st action is high
    prob_first_action = dist_instance.log_prob(torch.tensor([0]))

    # Make sure log_prob of other actions is low
    for i in range(1, act_size):
        assert dist_instance.log_prob(torch.tensor([i])) < prob_first_action
