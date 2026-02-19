using Rpg.Action;
using Rpg.AiStrategy;
using Rpg.Battle;
using Rpg.Core;
using Rpg.Game;
using Rpg.Observer;

var troop1 = new Troop(1) { Name = "玩家" };
var troop2 = new Troop(2) { Name = "敵軍" };

var hero = new Hero("英雄", 300, 500, 100)
{
    TroopId = 1
};
hero.Actions.Add(new BasicAttack());
hero.Actions.Add(new FireBall());
hero.Actions.Add(new WaterBall());
troop1.Allies.Add(hero);

var aiStrategy = new SeedSelectionStrategy();
var slime1 = new AI("Slime1", 150, 60, 49, aiStrategy)
{
    TroopId = 2
};
slime1.Actions.Add(new BasicAttack());
slime1.Actions.Add(new FireBall());
troop2.Allies.Add(slime1);

var slime2 = new AI("Slime2", 150, 200, 50, aiStrategy)
{
    TroopId = 2
};
slime2.Actions.Add(new BasicAttack());
slime2.Actions.Add(new FireBall());
slime2.Actions.Add(new WaterBall());
troop2.Allies.Add(slime2);

troop1.SetEnemy(troop2);
troop2.SetEnemy(troop1);

var game = new RpgGame();
game.RegisterObserver(new SlimeDeath());
game.RegisterObserver(new CurseDeath());

var context = new BattleContext(troop1, troop2, hero)
{
    Game = game
};

var runner = new BattleRunner(context);
var result = runner.Run(() => Console.ReadLine() ?? "");

if (result == BattleResult.PlayerWin)
    GameOutput.PrintVictory();
else
    GameOutput.PrintDefeat();
