using System.Web.Http.Filters;
using Moq;
using Teamworks.Web.Attributes.Api.Ordered;
using Xunit;

namespace Teamworks.Web.Unittest.Attributes.Api.Ordered
{
    public class OrderedFilterUnittest
    {
        [Fact]
        public void CompareTo()
        {
            var mock1 = new Mock<IOrderedFilter>();
            mock1.Setup(o => o.Order)
                .Returns(1);

            var mock2 = new Mock<IOrderedFilter>();
            mock2.Setup(o => o.Order)
                .Returns(2);

            var fi = new OrderedFilterInfo(mock1.Object, FilterScope.Global);
            int result = fi.CompareTo(new OrderedFilterInfo(mock2.Object, FilterScope.Global));
            Assert.Equal(-1, result);
        }

        [Fact]
        public void CompareIOrderedFilterToIFilter()
        {
            var mock1 = new Mock<IOrderedFilter>();
            mock1.Setup(o => o.Order)
                .Returns(1);

            var mock2 = new Mock<IFilter>();
            var fi = new OrderedFilterInfo(mock1.Object, FilterScope.Global);
            // mock1 precedes mock2 in the sort order
            Assert.Equal(-1, fi.CompareTo(new OrderedFilterInfo(mock2.Object, FilterScope.Global)));
        }

        [Fact]
        public void CompareIFilterToIFilter()
        {
            var mock1 = new Mock<IFilter>();
            var mock2 = new Mock<IFilter>();

            var fi = new OrderedFilterInfo(mock1.Object, FilterScope.Global);
            Assert.Equal(0, fi.CompareTo(new OrderedFilterInfo(mock2.Object, FilterScope.Global)));
        }

        [Fact]
        public void CompareToIFilterToIOrderedFilter()
        {
            var mock1 = new Mock<IFilter>();
            var mock2 = new Mock<IOrderedFilter>();
            mock2.Setup(o => o.Order)
                .Returns(1);

            var fi = new OrderedFilterInfo(mock1.Object, FilterScope.Global);
            Assert.Equal(1, fi.CompareTo(new OrderedFilterInfo(mock2.Object, FilterScope.Global)));
        }
    }
}