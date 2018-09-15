public interface IPlayerItem
{
    string SqLiteIndex { get; set; }
    string PlayerId { get; set; }
    string GUID { get; set; }
    int Amount { get; set; }
    int Exp { get; set; }
    string EquipItemId { get; set; }
    string EquipPosition { get; set; }

    
}
public interface IPlayerOtherItem
{
    string SqLiteIndex { get; set; }
    string PlayerId { get; set; }
    string DataId { get; set; }
    int Amount { get; set; }
    string Des { get; set; }
    int Exp { get; set; }

}