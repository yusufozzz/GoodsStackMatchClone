namespace Slots
{
    public sealed class SlotGameStateValidator
    {
        private BoxSlot[] _boxSlotsArray;
        private int _boxSlotsCount;

        public void Initialize(BoxSlot[] boxSlots)
        {
            _boxSlotsArray = boxSlots;
            _boxSlotsCount = boxSlots.Length;
        }

        /// <summary>
        /// Checks if the game is over by verifying if all box slots are occupied.
        /// </summary>
        /// <returns></returns>
        public bool IsGameOver()
        {
            for (var i = 0; i < _boxSlotsCount; ++i)
            {
                if (!_boxSlotsArray[i].IsOccupied)
                    return false;
            }
            
            return true;
        }
    }
}