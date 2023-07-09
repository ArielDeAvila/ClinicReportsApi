using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using ClinicReportsAPI.Validations.Register;
using Moq;
using System.Linq.Expressions;
using System.Xml;

namespace UnitTests.ServiceTests;

[TestClass]
public class HospitalServiceTest
{
    private readonly HospitalRegisterValidation _validation = new();

    [TestMethod]
    public async Task GetAll_Should_Return_Success_Response_With_All_Hospitals()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();
        
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetAll()).ReturnsAsync(GetTestHospital());

        var hospitalService = new HospitalService(unitOfWorkMock.Object,emailService.Object,_validation);

        var expectedResponse = new BaseResponse<IEnumerable<HospitalDTO>>
        {
            Success = true,
            Data = GetTestHospitalDto(),
            Message = ReplyMessage.MESSAGE_QUERY
        };

        //Act
        var response = await hospitalService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        var expectedData = expectedResponse.Data.ToList();
        var responseData = response.Data!.ToList();

        for (int i= 0; i < expectedData.Count; i++)
        {
            Assert.AreEqual(expectedData[i].Id, responseData[i].Id);  
            Assert.AreEqual(expectedData[i].Name, responseData[i].Name);  
        }


    }

    [TestMethod]
    public async Task GetAll_Should_Return_Failure_Response_When_Not_Found_Data()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();
        
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetAll()).ReturnsAsync((List<Hospital>)null);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<IEnumerable<HospitalDTO>>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        //Act
        var response = await hospitalService.GetAll();

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

    }

    [TestMethod]
    public async Task Create_Should_Return_Success_Response_When_Dto_Is_Valid()
    {

        //Arrange
        var emailService = new Mock<IEmailService>();
        emailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()))
                                                          .ReturnsAsync((Hospital)null);

        unitOfWorkMock.Setup(uow => uow.HospitalRepository
                                    .Create(It.IsAny<Hospital>(),It.IsAny<List<MedicalService>>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_SAVE
        };

        //Act
        var response = await hospitalService.Create(TestHopitalRegisterDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        unitOfWorkMock.Verify(
            uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()), Times.Exactly(2)
            );
        unitOfWorkMock.Verify(uow => uow.HospitalRepository
                                        .Create(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>()), Times.Once());
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once());


    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Email_Exists()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();
        emailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()))
                                                          .ReturnsAsync(new Hospital { Email = "hospital1@email.com" });

        unitOfWorkMock.Setup(uow => uow.HospitalRepository
                                    .Create(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).Verifiable();

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_EMAIL
        };

        //Act
        var response = await hospitalService.Create(TestHopitalRegisterDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        unitOfWorkMock.Verify(
            uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()), Times.Once
            );
        unitOfWorkMock.Verify(uow => uow.HospitalRepository
                                        .Create(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);

    }

    [TestMethod]
    public async Task Create_Should_Return_Failure_Response_When_Identification_Exists()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();
        emailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()))
                                .ReturnsAsync(new Hospital { Identification= "114553626459", Email = "new@email.com" });

        unitOfWorkMock.Setup(uow => uow.HospitalRepository
                                    .Create(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).Verifiable();

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION
        };

        //Act
        var response = await hospitalService.Create(TestHopitalRegisterDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        emailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        unitOfWorkMock.Verify(
            uow => uow.HospitalRepository.GetHospital(It.IsAny<Expression<Func<Hospital, bool>>>()), Times.Once
            );
        unitOfWorkMock.Verify(uow => uow.HospitalRepository
                                        .Create(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);

    }

    [TestMethod]
    public async Task Update_Should_Return_Success_Response_When_Data_Is_Correct()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetById(It.IsAny<int>()))
                                            .ReturnsAsync(TestHospital());
        unitOfWorkMock.Setup(uow => uow.HospitalRepository
                                        .Update(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_UPDATE
        };

        //Act
        var response = await hospitalService.Update(TestHospitalDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.HospitalRepository.GetById(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.HospitalRepository
                                        .Update(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);


    }

    [TestMethod]
    public async Task Update_Should_Return_Failure_Response_When_Hospital_Not_Exists()
    {
        //Arrange
        var emailService = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetById(It.IsAny<int>()))
                                            .ReturnsAsync((Hospital)null);
        unitOfWorkMock.Setup(uow => uow.HospitalRepository
                                        .Update(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        //Act
        var response = await hospitalService.Update(TestHospitalDto());

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.HospitalRepository.GetById(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.HospitalRepository
                                        .Update(It.IsAny<Hospital>(), It.IsAny<List<MedicalService>>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);


    }

    [TestMethod]
    public async Task Remove_Should_Return_Success_Response_When_Data_Is_Correct()
    {
        //Arrange
        int hospitalId = 1;

        var emailService = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetById(It.IsAny<int>())).ReturnsAsync(TestHospital());
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.Remove(It.IsAny<int>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = true,
            Data = true,
            Message = ReplyMessage.MESSAGE_DELETE
        };

        //Act
        var response = await hospitalService.Remove(hospitalId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.HospitalRepository.GetById(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.HospitalRepository.Remove(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

    }

    [TestMethod]
    public async Task Remove_Should_Return_Failure_Response_When_Hospital_Not_Exists()
    {
        //Arrange
        int hospitalId = 1;

        var emailService = new Mock<IEmailService>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.GetById(It.IsAny<int>())).ReturnsAsync((Hospital)null);
        unitOfWorkMock.Setup(uow => uow.HospitalRepository.Remove(It.IsAny<int>())).Verifiable();
        unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        var hospitalService = new HospitalService(unitOfWorkMock.Object, emailService.Object, _validation);

        var expectedResponse = new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_QUERY_EMPTY
        };

        //Act
        var response = await hospitalService.Remove(hospitalId);

        //Assert
        Assert.AreEqual(expectedResponse.Success, response.Success);
        Assert.AreEqual(expectedResponse.Data, response.Data);
        Assert.AreEqual(expectedResponse.Message, response.Message);

        unitOfWorkMock.Verify(uow => uow.HospitalRepository.GetById(It.IsAny<int>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.HospitalRepository.Remove(It.IsAny<int>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);

    }

    private IEnumerable<Hospital> GetTestHospital()
    {
        return new List<Hospital>
        {
            new Hospital { Id = 1, Name="Hospital1"},
            new Hospital { Id = 2, Name="Hospital2"},
            new Hospital { Id = 3, Name="Hospital3"}
        };
    }

    private IEnumerable<HospitalDTO> GetTestHospitalDto()
    {
        return new List<HospitalDTO>
        {
            new HospitalDTO { Id = 1, Name="Hospital1"},
            new HospitalDTO { Id = 2, Name="Hospital2"},
            new HospitalDTO { Id = 3, Name="Hospital3"}
        };
    }

    private HospitalRegisterDTO TestHopitalRegisterDto()
    {
        return new HospitalRegisterDTO
        {
            Name = "Hospital1",
            Address = "Mz 4 Crr 20",
            Email = "hospital1@email.com",
            Password = "1143256855",
            Identification = "114553626459",
            PhoneNumber = "3052524255",
            Services = new List<MedicalServiceDTO>
            {
                new MedicalServiceDTO { Id = 1, Name="Pediatria"},
                new MedicalServiceDTO { Id = 2, Name="Neurología"},
                new MedicalServiceDTO { Id = 3, Name="Odontología"}
            }
        };
    }

    private Hospital TestHospital()
    {
        return new Hospital
        {
            Id=1,
            Name = "Hospital1",
            Address = "Mz 4 Crr 20",
            Email = "hospital1@email.com",
            Password = "1143256855",
            Identification = "114553626459",
            PhoneNumber = "3052524255",
        };
    }

    private HospitalDTO TestHospitalDto()
    {
        return new HospitalDTO
        {
            Id = 1,
            Name = "Hospital1",
            Address = "Mz 4 Crr 20",
            Email = "hospital1@email.com",
            Identification = "114553626459",
            PhoneNumber = "3052524255",
            Services = new List<MedicalServiceDTO>
            {
                new MedicalServiceDTO { Id = 1, Name="Pediatria"},
                new MedicalServiceDTO { Id = 2, Name="Neurología"},
                new MedicalServiceDTO { Id = 3, Name="Odontología"}
            }
        };
    }

}
