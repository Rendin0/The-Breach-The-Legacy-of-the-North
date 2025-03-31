
public class Abilities<T> where T : UtilsAbilities
{
    // Поля для хранения вспомогательных утилит и сервиса существ
    protected static T utils;
    protected static CreaturesSerivce creaturesSerivce;

    public Abilities(CreaturesSerivce creatures)
    {
        creaturesSerivce = creatures;
    }
}