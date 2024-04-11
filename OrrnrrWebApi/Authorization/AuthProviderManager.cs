
namespace OrrnrrWebApi.Authorization
{
    public static class AuthProviderManager
    {
        private static string? _secretKey;
        public static string SecretKey
        {
            get => _secretKey ?? throw new ArgumentNullException(nameof(SecretKey), "secretKey 문자열이 초기화되지 않았습니다.");
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(SecretKey), value, $"{nameof(SecretKey)}의 값은 null이거나 비어있을 수 없습니다.");
                }

                if (_secretKey is not null)
                {
                    throw new InvalidOperationException("SecretKey는 중복으로 초기화될 수 없습니다.");
                }

                _secretKey ??= value;
            }
        }

        internal static JwtProvider CreateJwtProvider()
        {
            return new JwtProvider(SecretKey);
        }
    }
}
