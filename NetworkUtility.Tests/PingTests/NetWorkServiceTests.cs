using FluentAssertions;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetworkUtility.Tests.PingTests
{
    public class NetWorkServiceTests
    {
        [Fact]
        public void NetworkService_SendPing_ResultString()
        {
            //Arrange - varibles, classes, mocks
            var pingService = new NetworkService();

            //Act
            var result = pingService.SendPing();

            //Assert
            result.Should().NotBeNullOrWhiteSpace(); //check null hoặc có dấu cách
            result.Should().Be("Success: Ping sent!"); //so sánh bằng
            result.Should().Contain("Success",Exactly.Once()); //kiểm trs chứa
        }

        [Theory]
        [InlineData(1,1, 2)]//đặt các tham số truền vào và kết quả mong muốn
        [InlineData(2,2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        {
            //Arange
            var pingService = new NetworkService();

            //Act
            var result = pingService.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterThanOrEqualTo(2);//kiểm tra lớn hơn hoặc bằng
            result.Should().NotBeInRange(-100, 0);//check k tồn tại trong khoảng
        }
    }
}
