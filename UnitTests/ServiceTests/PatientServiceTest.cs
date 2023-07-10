using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using ClinicReportsAPI.Validations.Register;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.ServiceTests;

[TestClass]
public class PatientServiceTest
{
    private readonly PatientRegisterValidation _validator = new();

    [TestMethod]
    public async Task GetAll_Should_Return_Success_Respone_With_Patiens()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.GetAll()).ReturnsAsync(GetTestPatientList());

        var patientService = new PatientService(unitOfWorkMock.Object,emailSvc.Object,_validator);

        var expectedResponse = new BaseResponse<IEnumerable<PatientDTO>>
        {
            Success = true,
            Data = GetTestPatientDtoList(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        //Act
        var response = await patientService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        for (int i = 0; i < expectedData.Count; i++) 
        {
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);
            Assert.AreEqual(expectedData[i].Name, responseData[i].Name);
        }

        unitOfWorkMock.Verify(uow => uow.PatientRepository.GetAll(), Times.Once);

    }

    [TestMethod]
    public async Task GetAll_Should_Return_Failure_Response_When_NotFound_Patient()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.GetAll()).ReturnsAsync((List<Patient>)null);

        var patientService = new PatientService(unitOfWorkMock.Object, emailSvc.Object, _validator);

        var expectedResponse = new BaseResponse<IEnumerable<PatientDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        //Act
        var response = await patientService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.PatientRepository.GetAll(), Times.Once);

    }

    [TestMethod]
    public async Task Create_Should_Return_Success_Response_When_Data_Patient_Is_Valid()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();
        emailSvc.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()))
                                                            .ReturnsAsync((Patient)null);
        unitOfWorkMock.Setup(uow => uow.PatientRepository.Create(It.IsAny<Patient>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var patientService = new PatientService(unitOfWorkMock.Object,emailSvc.Object,_validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_SAVE
        };

        //Act
        var response = await patientService.Create(RegisterPatient());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailSvc.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()), Times.Exactly(2));
        unitOfWorkMock.Verify(uow => uow.PatientRepository.Create(It.IsAny<Patient>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Email_Exists()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();
        emailSvc.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()))
                                       .ReturnsAsync(new Patient { Email = "patient1@email.com" });
        unitOfWorkMock.Setup(uow => uow.PatientRepository.Create(It.IsAny<Patient>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var patientService = new PatientService(unitOfWorkMock.Object, emailSvc.Object, _validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_EMAIL
        };

        //Act
        var response = await patientService.Create(RegisterPatient());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailSvc.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.PatientRepository.Create(It.IsAny<Patient>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);

    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Identification_Exists()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();
        emailSvc.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()))
                            .ReturnsAsync(new Patient { Identification = "11343434234", Email = "newemail@email.com" });
        unitOfWorkMock.Setup(uow => uow.PatientRepository.Create(It.IsAny<Patient>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var patientService = new PatientService(unitOfWorkMock.Object, emailSvc.Object, _validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION
        };

        //Act
        var response = await patientService.Create(RegisterPatient());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailSvc.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.PatientRepository.GetPatient(It.IsAny<Expression<Func<Patient, bool>>>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.PatientRepository.Create(It.IsAny<Patient>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);

    }

    [TestMethod]
    public async Task Update_Should_Return_Success_Response_When_Data_Is_Updated_Successfully()
    {
        //Arrange
        var emailSvc = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.Update(It.IsAny<Patient>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var patientService = new PatientService(unitOfWorkMock.Object, emailSvc.Object, _validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_UPDATE
        };

        //Act
        var response = await patientService.Update(PatientDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.PatientRepository.Update(It.IsAny<Patient>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);


    }

    [TestMethod]
    public async Task Remove_Should_Return_Success_Response_When_Data_Is_Updated_Successfully()
    {
        //Arrange
        int patientId = 1;

        var emailSvc = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.PatientRepository.Remove(It.IsAny<int>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var patientService = new PatientService(unitOfWorkMock.Object, emailSvc.Object, _validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_DELETE
        };

        //Act
        var response = await patientService.Remove(patientId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.PatientRepository.Remove(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

    }

    private IEnumerable<Patient> GetTestPatientList()
    {
        return new List<Patient>
        {
            new Patient { Id=1, Name="patient1"},
            new Patient { Id=2, Name="patient2"},
            new Patient { Id=3, Name="patient3"}
        };
    }

    private IEnumerable<PatientDTO> GetTestPatientDtoList()
    {
        return new List<PatientDTO>
        {
            new PatientDTO { Id=1, Name="patient1"},
            new PatientDTO { Id=2, Name="patient2"},
            new PatientDTO { Id=3, Name="patient3"}
        };
    }

    private PatientRegisterDTO RegisterPatient()
    {
        return new PatientRegisterDTO
        {
            Email = "patient1@email.com",
            Password = "password",
            Identification = "11343434234",
            Address = "address",
            BirthDate = new DateTime(1996, 6, 24),
            Name = "Patient 1",
            PhoneNumber = "30524455466",
            Hospital = new HospitalNameDTO
            {
                Id = 1,
                Name = "hospital 1"
            }
        };
    }

    private PatientDTO PatientDto()
    {
        return new PatientDTO
        {
            Id = 1,
            Email = "patient1@email.com",
            Identification = "11343434234",
            Address = "address",
            BirthDate = new DateTime(1996, 6, 24),
            Name = "Patient 1",
            PhoneNumber = "30524455466",
            Hospital = new HospitalNameDTO
            {
                Id = 1,
                Name = "hospital 1"
            }
        };
    }
}
