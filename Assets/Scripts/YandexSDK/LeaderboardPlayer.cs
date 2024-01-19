namespace YandexSDK
{
    public class LeaderboardPlayer
    {
        public LeaderboardPlayer(string number, string name, string level)
        {
            Number = number;
            Name = name;
            Level = level;
        }

        public string Number { get; private set; }

        public string Name { get; private set; }

        public string Level { get; private set; }
    }
}
