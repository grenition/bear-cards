using Project;

namespace Assets.Scripts.Map
{
    internal class CraftPoint : InteractivePoint
    {
        public CraftPoint()
        {
            PointEntity.Key = "CraftMeadle";
        }
        public override void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCraftGiver();

            var data = DialoguesStatic.LoadData();
            data.CountCardCraftComming++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);
        }

        public override void OnEndInteract()
        {
        }
    }
}
