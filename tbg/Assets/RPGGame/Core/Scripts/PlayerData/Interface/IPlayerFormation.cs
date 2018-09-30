public interface IPlayerFormation
{
    string characterGuid { get; set; }
    string PlayerId { get; set; }
    string formationId { get; set; }
    int Position { get; set; }
    string ItemId { get; set; }
}
