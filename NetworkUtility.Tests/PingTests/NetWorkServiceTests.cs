using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
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
        private readonly IDNS _dns;
        public NetWorkServiceTests()
        {
            //Depedencies
            _dns = A.Fake<IDNS>();

            _pingService = new NetworkService(_dns);
        }

        [Fact]
        public void NetworkService_SendPing_ResultString()
        {
            A.CallTo(() => _dns.SendDNS()).Returns(true);
            //Act
            var result = _pingService.SendPing();

            //Assert
            result.Should().NotBeNullOrWhiteSpace(); //check null hoặc có dấu cách
            result.Should().Be("Success: Ping sent!"); //so sánh bằng
            result.Should().Contain("Success",Exactly.Once()); //kiểm trs chứa
        }

        [Theory] //đánh dấu loại kiểm tra dùng nhiều trường hợp kiểm thử
        //InlineData đi cùng với [Theory]
        //Mỗi InlineData là một trường hợp kiểm thử
        //Tham số truyền vào sẽ là các biến truyền vào hàm và nên có một expected result
        [InlineData(1,1, 2)]
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
            var result = _pingService.LastPingDate();
            //Assert
            result.Should().BeAfter(1.January(2010)); //kiểm tra thời gian lớn hơn  ngày 1 tháng1 năm 2010
            result.Should().BeBefore(1.January(2030)); //kiểm tra thời gian nhỏ hơn  ngày 1 tháng 1 năm 2030
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
            result.Should().BeOfType<PingOptions>(); //kiểm tra kiểu dữ liệu của đối tượng
            result.Should().BeEquivalentTo(expected); //so sánh giá trị của hai cấu trúc dữ liệu, bất kể thứ tự các phần tử, dùng để so sánh các obj có cấu trúc phức tạp
            result.Ttl.Should().Be(1);//so sánh bằng giá trị
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
            result.Should().ContainEquivalentOf(expected); //kiểm tra danh sách có chứa phần tử hay không
            result.Should().Contain(x=>x.DontFragment == true); //kiểm tra danh sách obj có chứa thuộc tính giá trị mong đợi hay không
        }
    }
}
