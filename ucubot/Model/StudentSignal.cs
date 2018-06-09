namespace ucubot.Model
{
    public class StudentSignal
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SignalType { get; set; }
        public int Count { get; set; }

        public override bool Equals(object obj)
        {
            var StudentEnother = (StudentSignal) obj;
            return StudentEnother.Count == Count&& StudentEnother.FirstName == StudentEnother.FirstName&& StudentEnother.LastName == LastName&& StudentEnother.SignalType == SignalType;
        }
    }
}