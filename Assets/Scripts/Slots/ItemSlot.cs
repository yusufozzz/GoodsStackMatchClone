using Items;

namespace Slots
{
    public class ItemSlot: SlotBase<Item>
    {
        public override void SetContent(Item o)
        {
            base.SetContent(o);
            o.SetTransform(transform);
        }
    }
}