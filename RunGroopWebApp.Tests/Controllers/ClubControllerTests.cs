using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Controllers
{
    public class ClubControllerTests
    {

        private ClubController _clubController;
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpAssetContext _httpContext;
        
        public ClubControllerTests()
        {
            //depen
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContext = A.Fake<IHttpAssetContext>();


            _clubController = new ClubController(_clubRepository, _photoService, _httpContext);
        }

        [Fact]
        public void ClubController_Index_ReturnSuccess()
        {
            //Arrange - what do i need to bring to?
            var clubs = A.Fake<IEnumerable<Club>>();
            //Act
            A.CallTo(()=> _clubRepository.GetAll()).Returns(clubs);

            var result = _clubController.Index();
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();

        }

        [Fact]
        public void ClubController_Detail_ReturnSuccess()
        {
            //Arrange - what do i need to bring to?
            int id = 1;
            var clubs = A.Fake<Club>();
            //Act
            A.CallTo(() => _clubRepository.GetIdAsyns(id)).Returns(clubs);

            var result = _clubController.Detail(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();

        }
    }
}
