using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;

namespace platformAthletic.UnitTest.Mock.Http
{
    public class MockHttpCachePolicy : Mock<HttpCachePolicyBase>
    {
        public MockHttpCachePolicy(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            
        }
    }
}
