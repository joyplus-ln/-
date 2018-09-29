public interface IPlayerItem
{
    string ItemID { get; set; }
    string PlayerId { get; set; }
    string GUID { get; set; }
    int Amount { get; set; }
    int Exp { get; set; }
    string EquipItemId { get; set; }
    string EquipPosition { get; set; }
    PlayerItem.ItemType itemType { get; set; }


}
public interface IPlayerOtherItem
{
    string ItemID { get; set; }
    string PlayerId { get; set; }
    string DataId { get; set; }
    int Amount { get; set; }
    string Des { get; set; }
    int Exp { get; set; }

}