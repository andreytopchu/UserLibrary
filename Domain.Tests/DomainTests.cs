using Domain.Common.Errors;
using Domain.Entities;
using Domain.Entities.Enum;

namespace Domain.Tests;

public class DomainTests
{
    private readonly List<User> _users = new();
    private readonly List<Role> _roles = new();
    private readonly List<WorkflowStep> _steps = new();
    private Applicant _applicant = null!;
    private Workflow _applicantWorkflow = null!;

    [SetUp]
    public void Setup()
    {
        _roles.AddRange(new List<Role>
        {
            new("Admin"),
            new("HR"),
            new("Specialist"),
            new("Chief")
        });

        _users.AddRange(new List<User>
        {
            new("Alex Kondratuk", _roles[0]),
            new("Vasiliy Pavlovich", _roles[1]),
            new("Dmitriy Carp", _roles[2]),
            new("Evgeniy Serebkov", _roles[3]),
        });

        for (var i = 0; i < _users.Count; i++)
        {
            _steps.Add(new WorkflowStep(_users[i], i));
        }

        for (var i = 0; i < _roles.Count; i++)
        {
            _steps.Add(new WorkflowStep(_roles[i], _users.Count + i));
        }

        _applicantWorkflow = new Workflow(_steps);

        _applicant = new Applicant(_users.First(), _applicantWorkflow,
            new Document("Alex", "Vasiliev", DateTime.Today.AddYears(-30), "+123456789", "work"));
    }

    [Test]
    public void ApprovedApplicant_PositiveTest()
    {
        foreach (var user in _users)
        {
            _applicant.Approve(user);
        }

        foreach (var role in _roles)
        {
            _applicant.Approve(_users.First(x => x.Role.Equals(role)));
        }

        Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.Approved));
    }

    [Test]
    public void TryApproveAlreadyCompletedApplicant_NegativeTest()
    {
        foreach (var user in _users)
        {
            _applicant.Approve(user);
        }

        foreach (var role in _roles)
        {
            _applicant.Approve(_users.First(x => x.Role.Equals(role)));
        }

        Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.Approved));

        var result = _applicant.Approve(_users.First());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(Errors.Workflow.IsAlreadyCompleted));
        });
    }

    [Test]
    public void AddStepInWorkflowAfterApproved_PositiveTest()
    {
        foreach (var user in _users)
        {
            _applicant.Approve(user);
        }

        foreach (var role in _roles)
        {
            _applicant.Approve(_users.First(x => x.Role.Equals(role)));
        }

        Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.Approved));

        _applicantWorkflow.AddStep(_users.First());

        Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.InProgress));
    }

    [Test]
    public void RejectedApplicant_PositiveTest()
    {
        var result = _applicant.Reject(_users.First());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsError, Is.False);
            Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.Rejected));
        });
    }

    [Test]
    public void ApprovedAfterRejectedApplicant_NegativeTest()
    {
        var rejectedResult = _applicant.Reject(_users.First());

        Assert.Multiple(() =>
        {
            Assert.That(rejectedResult.IsError, Is.False);
            Assert.That(_applicant.Status, Is.EqualTo(ApplicantStatus.Rejected));
        });

        var approveResult = _applicant.Approve(_users.First());

        Assert.Multiple(() =>
        {
            Assert.That(approveResult.IsError, Is.True);
            Assert.That(approveResult.FirstError, Is.EqualTo(Errors.Workflow.IsAlreadyRejected));
        });
    }

    [Test]
    public void ResetApprovedUser()
    {
        Assert.That(_applicantWorkflow.CurrentStepNumber, Is.EqualTo(0));

        foreach (var user in _users)
        {
            _applicant.Approve(user);
        }

        Assert.That(_applicantWorkflow.CurrentStepNumber, Is.EqualTo(_users.Count));

        _applicantWorkflow.Reset(_users.First());

        Assert.That(_applicantWorkflow.CurrentStepNumber, Is.EqualTo(0));
    }

    [Test]
    public void GetStatusLogs()
    {
        foreach (var user in _users)
        {
            _applicant.Approve(user);
        }

        foreach (var role in _roles)
        {
            _applicant.Approve(_users.First(x => x.Role.Equals(role)));
        }

        foreach (var i in _applicantWorkflow.Logs)
        {
            Console.WriteLine(i);
        }
    }
}