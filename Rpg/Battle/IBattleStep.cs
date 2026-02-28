namespace Rpg.Battle;

public interface IBattleStep
{
    bool CanExecute(TakeTurnContext context);
    IBattleStep? Execute(TakeTurnContext context);
}
