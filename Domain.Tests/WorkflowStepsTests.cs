using Domain.Entities;

namespace Domain.Tests;

public class WorkflowStepsTests
{
    [Test]
    public void IsCanApproveWithUser_PositiveTest()
    {
        var user = new User("Alex Vasiliev", new Role("Chef"));
        var workflowStep = new WorkflowStep(user, 1);

        Assert.That(workflowStep.IsCanApprove(user), Is.True);
    }
    
    [Test]
    public void IsCanApproveWithRole_PositiveTest()
    {
        var role = new Role("Chef");
        var user = new User("Alex Vasiliev", role);
        var workflowStep = new WorkflowStep(role, 1);

        Assert.That(workflowStep.IsCanApprove(user), Is.True);
    }

    [Test]
    public void TryApproveWithInvalidUser_NegativeTest()
    {
        var user = new User("Alex Vasiliev", new Role("Manager"));
        var workflowStep = new WorkflowStep(new User("Alexander Vasiliev", new Role("Chef")), 1);

        Assert.That(workflowStep.IsCanApprove(user), Is.False);
    }

    [Test]
    public void TryApproveWithInvalidRole_NegativeTest()
    {
        var role = new Role("Manager");
        var user = new User("Alex Vasiliev", new Role("Chef"));
        var workflowStep = new WorkflowStep(role, 1);

        Assert.That(workflowStep.IsCanApprove(user), Is.False);
    }
}