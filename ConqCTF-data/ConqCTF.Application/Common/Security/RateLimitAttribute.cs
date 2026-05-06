namespace ConqCTF.Application.Common.Security
{
    /// <summary>
    /// Specifies which rate limit type to be used.
    /// Use PerUse for authenticated access.
    /// </summary>
    public enum RateLimitType
    {
        PerUser,
        PerIp,
        PerIdentifier // email
    }

    /// <summary>
    /// Specifies the class this attribute is applied to rate limitation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RateLimitAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RateLimitAttribute"/> class. 
        /// </summary>
        public RateLimitAttribute() { }

        /// <summary>
        /// Gets or sets the maximum requests a user can make.
        /// </summary>
        public int MaxRequests { get; set; }

        /// <summary>
        /// Gets or sets the period of the rate limit.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets the rate limit type.
        /// </summary>
        public RateLimitType Type { get; set; }

    }
}
