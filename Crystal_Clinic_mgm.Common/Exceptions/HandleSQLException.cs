namespace Crystal_Clinic_Mgm.Common.Exceptions
{
    public class HandleSQLException : Exception
    {
        public HandleSQLException()
            : base("You are not connected with database please check your connection or network.")
        {
        }
        public HandleSQLException(string message)
            : base(message)
        {
        }
        //public IDictionary<string, string[]> Errors { get; }
        //public HandleSQLException()
        //    : base("You are not connected with database please check your connection or network.")
        //{
        //    Errors = new Dictionary<string, string[]>();
        //}
    }
}
