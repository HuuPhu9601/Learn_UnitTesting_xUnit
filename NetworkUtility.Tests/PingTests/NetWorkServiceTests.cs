using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetworkUtility.Tests.PingTests
{
    public class NetWorkServiceTests
    {
        private readonly NetworkService _pingService;

        public NetWorkServiceTests()
        {
            _pingService = new NetworkService();
        }

        [Fact]
        public void NetworkService_SendPing_ResultString()
        {

            //Act
            var result = _pingService.SendPing();

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

            //Act
            var result = _pingService.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterThanOrEqualTo(2);//kiểm tra lớn hơn hoặc bằng
            result.Should().NotBeInRange(-100, 0);//check k tồn tại trong khoảng
        }

        [Fact]
        public void NetworkService_lastpingDate_ReturnDate()
        {

            //Act
            var result = _pingService.LastPingDate();

            //Assert
            result.Should().BeAfter(1.January(2010));
            result.Should().BeBefore(1.January(2030));
        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnObject()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };
            var result = _pingService.GetPingOptions();

            //Assert
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnIEnumerable()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };
            var result = _pingService.MostRecentPings();

            //Assert
            //result.Should().BeOfType<IEnumerable<PingOptions>>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x=>x.DontFragment == true);
        }
    }
}
