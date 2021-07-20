namespace Router.Contracts
{
    public readonly struct VersionRange
    {
        public Version From { get; }

        public Version To { get; }

        public VersionRange(Version fromVersion, Version toVersion)
        {
            From = fromVersion;
            To = toVersion;
        }

        public bool IntersectsWith(VersionRange range)
        {
            var a = range.From >= From && range.From <= To;
            var b = range.To >= From && range.To <= To;
            return a || b;
        }
        
        public override string ToString()
        {
            return $"{From.ToString()}{To.ToString()}";
        }
    }
}