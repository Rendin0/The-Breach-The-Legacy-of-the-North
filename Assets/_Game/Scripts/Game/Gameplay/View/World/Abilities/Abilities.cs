
public class Abilities<T> where T : UtilsAbilities
{
    // ���� ��� �������� ��������������� ������ � ������� �������
    protected static T utils;
    protected static CreaturesSerivce creaturesSerivce;

    public Abilities(CreaturesSerivce creatures)
    {
        creaturesSerivce = creatures;
    }
}