﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using platformAthletic.Model;

namespace platformAthletic.UnitTest.Mock
{
    public partial class MockRepository : Mock<IRepository>
    {
        public MockRepository(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            GenerateRoles();
            GenerateUsers();
        }
    }
}
