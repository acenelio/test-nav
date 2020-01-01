using UnityEngine;

namespace NavGame.Character
{
    public interface IReachable
    {
        float ContactRadius();

        GameObject ToGameObject();
    }
}
