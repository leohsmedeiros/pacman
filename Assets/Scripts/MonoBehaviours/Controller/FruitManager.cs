using UnityEngine;

public class FruitManager : MonoBehaviour {
    public Fruit cherryPrefab;
    public Fruit strawberryPrefab;
    public Fruit orangePrefab;
    public Fruit applePrefab;
    public Fruit melonPrefab;
    public Fruit galaxianPrefab;
    public Fruit bellPrefab;
    public Fruit keyPrefab;

    public Node referenceNode;

    private GameObject _fruitInstantiated;


    public Fruit GetFruitByType(FruitType fruitType) {
        switch (fruitType) {
            default: return null;
            case FruitType.CHERRY: return cherryPrefab;
            case FruitType.STRAWBERRY: return strawberryPrefab;
            case FruitType.ORANGE: return orangePrefab;
            case FruitType.APPLE: return applePrefab;
            case FruitType.MELON: return melonPrefab;
            case FruitType.GALAXIAN: return galaxianPrefab;
            case FruitType.BELL: return bellPrefab;
            case FruitType.KEY: return keyPrefab;
        }
    }

    public void InstantiateFruit(FruitType fruitType) {
        Fruit selectedFruit = GetFruitByType(fruitType);

        if (selectedFruit != null) {
            _fruitInstantiated = Instantiate(selectedFruit.gameObject,
                                             referenceNode.GetPosition2D(),
                                             Quaternion.identity);
        }
    }

    public void DestroyFruit() {
        if (_fruitInstantiated != null) {
            Destroy(_fruitInstantiated);
        }
    }

}
