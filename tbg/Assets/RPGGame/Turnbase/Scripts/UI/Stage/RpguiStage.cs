public class RpguiStage : BaseRpguiStage<RpguiStagePreparation, NormalStage>
{
    public RpguiStagePreparation RpguiStagePreparation;
    public override RpguiStagePreparation StagePreparation
    {
        get { return RpguiStagePreparation; }
    }
}
