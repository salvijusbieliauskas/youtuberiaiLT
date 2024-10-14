namespace Services.YoutubeAPI.Helpers.Wrappers
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        protected Result(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
        public static Result Err(string message)
        {
            return new Result(false, string.Empty);
        }
        public static Result<T> Err<T>(string message)
        {
            return new Result<T>(default, false, message);
        }
        public static implicit operator bool(Result result)
        {
            return result.Success;
        }
        public virtual Result Append(string message)
        {
            this.Message += "\n" + message;
            return this;
        }
    }
    public class Result<T> : Result
    {
        public T Value { get; private set; }
        protected internal Result(T value, bool success, string message)
        : base(success,message)
        {
            this.Value = value;
        }

        public override Result<T> Append(string message)
        {
            base.Append(message);
            return this;
        }
    }
}
