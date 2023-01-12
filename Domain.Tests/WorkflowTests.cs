using Domain.Entities;

namespace Domain.Tests;

public class WorkflowTests
{
    [Test]
    public void AddStepWithUser_PositiveTest()
    {
        var workflow = new Workflow(new List<WorkflowStep> { new(new User("Vladimir", new Role("Manager")), 1) });

        workflow.AddStep(new User("Vladimir", new Role("Manager")));

        Assert.That(workflow.Steps, Has.Count.EqualTo(2));
    }

    [Test]
    public void AddStepWithRole_PositiveTest()
    {
        var workflow = new Workflow(new List<WorkflowStep> { new(new User("Vladimir", new Role("Manager")), 1) });

        workflow.AddStep(new Role("Manager"));

        Assert.That(workflow.Steps, Has.Count.EqualTo(2));
    }
}