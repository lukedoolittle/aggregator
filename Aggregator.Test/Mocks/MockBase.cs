﻿using LightMock;

namespace Aggregator.Test.Helpers.Mocks
{
    public abstract class MockBase<TMock>
    {
        protected readonly MockContext<TMock> _context;
        protected readonly IInvocationContext<TMock> _invoker; 

        protected MockBase()
        {
            _context = new MockContext<TMock>();
            _invoker = _context;
        } 
    }
}
