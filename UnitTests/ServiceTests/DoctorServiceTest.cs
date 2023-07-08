using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using ClinicReportsAPI.Validations.Register;
using FluentValidation;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.ServiceTests;

[TestClass]
public class DoctorServiceTest
{
    [TestMethod]
    public async Task GetAll_Should_Return_Success_Response_With_Doctors()
    {
        //Arrange
        var emailServiceMock = new Mock<IEmailService>();
        var validatorMock = new Mock<IValidator<DoctorRegisterDTO>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.GetAll()).ReturnsAsync(GetDoctors());

        var doctorService = new DoctorService(unitOfWorkMock.Object,emailServiceMock.Object,validatorMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<DoctorDTO>>
        {
            Success = true,
            Data = GetTestDoctors(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        //Act
        var response = await doctorService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        for(int i=0; i<responseData.Count; i++)
        {
            Assert.AreEqual(expectedData[i].Name, responseData[i].Name);
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);
        }
        

    }

    [TestMethod]
    public async Task GetAll_Should_Return_Failure_Response_When_Doctors_Are_Not_Found()
    {
        //Arrange
        var emailServiceMock = new Mock<IEmailService>();
        var validatorMock = new Mock<IValidator<DoctorRegisterDTO>>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.GetAll()).ReturnsAsync((List<Doctor>)null);

        var doctorService = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validatorMock.Object);

        var expectedResponse = new BaseResponse<IEnumerable<DoctorDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        //Act
        var response = await doctorService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);
    }

    [TestMethod]
    public async Task Create_Should_Return_Success_Reponse_When_DoctorDto_Is_Correct()
    {
        //Arrange
        var validator = new RegisterDoctorValidation();       

        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock.Setup(emailS => emailS.SendEmail(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>()))
                            .Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.Create(It.IsAny<Doctor>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.GetDoctor(It.IsAny<Expression<Func<Doctor,bool>>>()))
            .ReturnsAsync((Doctor)null);
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var doctorSerivce = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_SAVE
        };

        //Act
        var response = await doctorSerivce.Create(DataRegisterDoctorDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.DoctorRepository.Create(It.IsAny<Doctor>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Email_Exists_On_DB()
    {
        //Arrange
        var validator = new RegisterDoctorValidation();

        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock.Setup(emailS => emailS.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.Create(It.IsAny<Doctor>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.GetDoctor(It.IsAny<Expression<Func<Doctor, bool>>>()))
            .ReturnsAsync(new Doctor { Email= "doctor1@email.com" });
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).Verifiable();

        var doctorService = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_EMAIL
        };

        //Act
        var response = await doctorService.Create(DataRegisterDoctorDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(ouw => ouw.DoctorRepository.Create(It.IsAny<Doctor>()),Times.Never());
        unitOfWorkMock.Verify(ouw => ouw.CommitAsync(),Times.Never());

    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Identification_Exists_On_DB()
    {
        //Arrange
        var validator = new RegisterDoctorValidation();

        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock.Setup(emailS => emailS.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.Create(It.IsAny<Doctor>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.GetDoctor(It.IsAny<Expression<Func<Doctor, bool>>>()))
            .ReturnsAsync(new Doctor { Identification = "1143392491", Email="new@email.com" });
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).Verifiable();

        var doctorService = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION
        };

        //Act
        var response = await doctorService.Create(DataRegisterDoctorDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(ouw => ouw.DoctorRepository.Create(It.IsAny<Doctor>()), Times.Never());
        unitOfWorkMock.Verify(ouw => ouw.CommitAsync(), Times.Never());

    }

    [TestMethod]
    public async Task Update_Should_Return_Success_Response()
    {
        //Arrange
        var validator = new RegisterDoctorValidation();

        var emailServiceMock = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.Update(It.IsAny<Doctor>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var doctorService = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_UPDATE
        };

        //Act
        var response = await doctorService.Update(DataDoctorDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Once());
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once());


    }

    [TestMethod]
    public async Task Remove_Should_Return_Success_Respnse()
    {
        //Arrange
        int DoctorId = 1;

        var validator = new RegisterDoctorValidation();

        var emailServiceMock = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.DoctorRepository.Remove(It.IsAny<int>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var doctorService = new DoctorService(unitOfWorkMock.Object, emailServiceMock.Object, validator);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_DELETE
        };

        //Act
        var response = await doctorService.Remove(DoctorId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.DoctorRepository.Remove(It.IsAny<int>()), Times.Once());
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once());


    }

    private IEnumerable<Doctor> GetDoctors()
    {
        return new List<Doctor>
        {
            new Doctor { Id=1, Name="doctor1" },
            new Doctor { Id=2, Name="doctor2" },
            new Doctor { Id=3, Name="doctor3" }
        };
    }

    private IEnumerable<DoctorDTO> GetTestDoctors()
    {
        return new List<DoctorDTO> 
        { 
            new DoctorDTO { Id = 1, Name = "doctor1" },
            new DoctorDTO { Id = 2, Name = "doctor2" },
            new DoctorDTO { Id = 3, Name = "doctor3" }
        };
    }

    private DoctorRegisterDTO DataRegisterDoctorDto()
    {
        return new DoctorRegisterDTO
        {
            Name = "doctor1",
            Email = "doctor1@email.com",
            Identification = "1143392491",
            PhoneNumber = "3052524255",
            Address = "Dg 32 N 70 50",
            MedicalSpecialty = "Pediatra",
            BirthDate = new DateTime(1996, 6, 24)
        };
    }

    private DoctorDTO DataDoctorDto()
    {
        return new DoctorDTO
        {
            Id = 1,
            Name = "doctor1",
            Email = "doctor1@email.com",
            Identification = "1143392491",
            PhoneNumber = "3052524255",
            Address = "Dg 32 N 70 50",
            MedicalSpecialty = "Pediatra",
            BirthDate = new DateTime(1996, 6, 24),
            Hospital = new HospitalNameDTO
            {
                Id = 1,
                Name = "Hospital1"
            }
        };
    }

}
