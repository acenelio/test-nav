using UnityEngine;

namespace NavGame.Character
{
    public interface INavigable
    {
        float ContactRadius();

        GameObject ToGameObject();
    }
}
