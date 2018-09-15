public class UIStage : BaseUIStage<UIStagePreparation, NormalStage>
{
    public UIStagePreparation uiStagePreparation;
    public override UIStagePreparation StagePreparation
    {
        get { return uiStagePreparation; }
    }
}
