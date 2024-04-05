using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLib.Models
{
    public class ContextManager
    {
        private string? _orrnrrConnectionString;
        private static ContextManager? _instance;
        public static ContextManager Instance { get => _instance is null ? _instance = new ContextManager() : _instance; }
        public OrrnrrContext OrrnrrContext
        {
            get
            {
                if (string.IsNullOrEmpty(_orrnrrConnectionString))
                {
                    throw new InvalidOperationException("연결문자열이 초기화되지 않아 컨텍스트를 생성할 수 없습니다.");
                }

                return new OrrnrrContext(_orrnrrConnectionString);
            }
        }
        private ContextManager() { }

        public void UseOrrnrrContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("연결문자열은 null이거나 비어있을 수 없습니다.");
            }

            _orrnrrConnectionString = connectionString;
        }
    }
}
