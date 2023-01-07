using Application.Command;
using Application.Handlers;
using Application.Repository;
using Domain.Entities;
using Moq;

namespace Application.Tests;

public class ApplicationTests
{
    [Test]
    public void AddWorkflowStepWithUser_PositiveTest()
    {
        var workflow = new Workflow(new List<WorkflowStep> { new(new Role("MyRole"), 1), new(new Role("YourRole"), 1) });
        var document = new Document("Anton", "Chelak", DateTime.Now, "+78945612332", "work");
        var applicant = new Applicant(new User("Vladimir Volga", new Role("Chief")), workflow, document);
        var user = new User("Valery Miladze", new Role("Specialist"));

        var applicantMock = new Mock<IApplicantRepository>(MockBehavior.Strict);
        var userMock = new Mock<IUserRepository>(MockBehavior.Strict);
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addStepHandler = new AddWorkflowStepHandler(unitOfWorkMock.Object);
        var addStepCommand = new AddWorkflowStepByUserCommand(user.Id, applicant.Id);

        applicantMock.Setup(m => m.GetById(applicant.Id)).Returns(applicant);
        userMock.Setup(m => m.GetById(user.Id)).Returns(user);
        unitOfWorkMock.Setup(m => m.GetApplicantRepository()).Returns(applicantMock.Object);
        unitOfWorkMock.Setup(m => m.GetUserRepository()).Returns(userMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());
        userMock.Setup(m => m.GetById(user.Id)).Returns(user);

        addStepHandler.Handle(addStepCommand);

        applicantMock.Verify(m => m.GetById(It.IsAny<Guid>()));
        unitOfWorkMock.Verify(m => m.GetApplicantRepository());
        unitOfWorkMock.Verify(m => m.GetUserRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        userMock.Verify(m => m.GetById(It.IsAny<Guid>()));
    }

    [Test]
    public void AddWorkflowStepWithRole_PositiveTest()
    {
        var workflow = new Workflow(new List<WorkflowStep> { new(new Role("MyRole"), 1), new(new Role("YourRole"), 1) });
        var document = new Document("Anton", "Chelak", DateTime.Now, "+78945612332", "work");
        var applicant = new Applicant(new User("Vladimir Volga", new Role("Chief")), workflow, document);
        var role = new Role("Specialist");

        var applicantMock = new Mock<IApplicantRepository>(MockBehavior.Strict);
        var roleMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var addStepHandler = new AddWorkflowStepHandler(unitOfWorkMock.Object);
        var addStepCommand = new AddWorkflowStepByRoleCommand(role.Id, applicant.Id);

        applicantMock.Setup(m => m.GetById(applicant.Id)).Returns(applicant);
        roleMock.Setup(m => m.GetById(role.Id)).Returns(role);
        unitOfWorkMock.Setup(m => m.GetApplicantRepository()).Returns(applicantMock.Object);
        unitOfWorkMock.Setup(m => m.GetRoleRepository()).Returns(roleMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());
        roleMock.Setup(m => m.GetById(role.Id)).Returns(role);

        addStepHandler.Handle(addStepCommand);

        applicantMock.Verify(m => m.GetById(It.IsAny<Guid>()));
        unitOfWorkMock.Verify(m => m.GetApplicantRepository());
        unitOfWorkMock.Verify(m => m.GetRoleRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        roleMock.Verify(m => m.GetById(It.IsAny<Guid>()));
    }

    [Test]
    public void ApproveStep_PositiveTest()
    {
        var role = new Role("MyRole");
        var workflow = new Workflow(new List<WorkflowStep> { new(new Role("MyRole"), 1) });
        var document = new Document("Anton", "Chelak", DateTime.Now, "+78945612332", "work");
        var applicant = new Applicant(new User("Vladimir Volga", new Role("Chief")), workflow, document);
        var user = new User("Valery Miladze", role);

        var applicantMock = new Mock<IApplicantRepository>(MockBehavior.Strict);
        var userContextMock = new Mock<IUserContext>(MockBehavior.Strict);
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var approveStepHandler = new ApproveStepHandler(unitOfWorkMock.Object, userContextMock.Object);
        var approveStepCommand = new ApproveStepCommand(applicant.Id);

        applicantMock.Setup(m => m.GetById(It.Is<Guid>((x) => x == applicant.Id))).Returns(applicant);
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(user);
        unitOfWorkMock.Setup(m => m.GetApplicantRepository()).Returns(applicantMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());

        approveStepHandler.Handle(approveStepCommand);

        applicantMock.Verify(m => m.GetById(It.IsAny<Guid>()));
        unitOfWorkMock.Verify(m => m.GetApplicantRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        userContextMock.Verify(m => m.GetCurrentUser());
    }

    [Test]
    public void RejectStep_PositiveTest()
    {
        var role = new Role("MyRole");
        var workflow = new Workflow(new List<WorkflowStep> { new(new Role("MyRole"), 1) });
        var document = new Document("Anton", "Chelak", DateTime.Now, "+78945612332", "work");
        var applicant = new Applicant(new User("Vladimir Volga", new Role("Chief")), workflow, document);
        var user = new User("Valery Miladze", role);

        var applicantMock = new Mock<IApplicantRepository>(MockBehavior.Strict);
        var userContextMock = new Mock<IUserContext>(MockBehavior.Strict);
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var rejectStepHandler = new RejectStepHandler(unitOfWorkMock.Object, userContextMock.Object);
        var rejectStepCommand = new RejectStepCommand(applicant.Id);

        applicantMock.Setup(m => m.GetById(applicant.Id)).Returns(applicant);
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(user);
        unitOfWorkMock.Setup(m => m.GetApplicantRepository()).Returns(applicantMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());

        rejectStepHandler.Handle(rejectStepCommand);

        applicantMock.Verify(m => m.GetById(It.IsAny<Guid>()));
        unitOfWorkMock.Verify(m => m.GetApplicantRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        userContextMock.Verify(m => m.GetCurrentUser());
    }

    [Test]
    public void CreateApplicant_PositiveTest()
    {
        var role = new Role("MyRole");
        var workflow = new Workflow(new List<WorkflowStep> { new(new Role("MyRole"), 1) });
        var document = new Document("Anton", "Chelak", DateTime.Now, "+78945612332", "work");
        var user = new User("Valery Miladze", role);

        var applicantMock = new Mock<IApplicantRepository>(MockBehavior.Strict);
        var userContextMock = new Mock<IUserContext>(MockBehavior.Strict);
        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var createApplicantHandler = new CreateApplicantHandler(unitOfWorkMock.Object, userContextMock.Object);
        var createApplicantCommand = new CreateApplicantCommand(workflow, document);

        applicantMock.Setup(m => m.Create(It.Is<Applicant>(param => param != null)))
            .Returns(It.Is<Guid>(_ => true));
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(user);
        unitOfWorkMock.Setup(m => m.GetApplicantRepository()).Returns(applicantMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());

        createApplicantHandler.Handle(createApplicantCommand);

        unitOfWorkMock.Verify(m => m.GetApplicantRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        userContextMock.Verify(m => m.GetCurrentUser());
    }

    [Test]
    public void CreateUser_PositiveTest()
    {
        var role = new Role("MyRole");
        var user = new User("Valery Miladze", role);

        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        var roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        var createUserHandler = new CreateUserHandler(unitOfWorkMock.Object);
        var createUserCommand = new CreateUserCommand("Valery Miladze", role.Id);

        roleRepositoryMock.Setup(m => m.GetById(role.Id)).Returns(role);
        userRepositoryMock.Setup(m => m.Create(user)).Returns(user.Id);
        unitOfWorkMock.Setup(m => m.GetRoleRepository()).Returns(roleRepositoryMock.Object);
        unitOfWorkMock.Setup(m => m.GetUserRepository()).Returns(userRepositoryMock.Object);
        unitOfWorkMock.Setup(m => m.Commit());

        createUserHandler.Handle(createUserCommand);

        unitOfWorkMock.Verify(m => m.GetUserRepository());
        unitOfWorkMock.Verify(m => m.GetRoleRepository());
        unitOfWorkMock.Verify(m => m.Commit());
        roleRepositoryMock.Verify(m => m.GetById(It.IsAny<Guid>()));
        userRepositoryMock.Verify(m => m.Create(It.IsAny<User>()));
    }
}