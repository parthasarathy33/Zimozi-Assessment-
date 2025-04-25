using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementAPI.Tests.Helpers
{
    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<T>(queryableData.Provider));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<T>(queryableData.GetEnumerator()));

            return mockDbSet;
        }
    }
} 