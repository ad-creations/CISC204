using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 3;
    public const int gridCols = 3;
    public const float offsetX = 4f;
    public const float offsetY = 3.5f;
    private int _score = 0;
    private int _turns = 0;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh turnsLabel;
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    public bool canReveal {
        get {return _secondRevealed == null;}

    }
    public void CardRevealed(MemoryCard card){
        if(_firstRevealed == null){
            _firstRevealed = card;

        }else{
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }
    private IEnumerator CheckMatch(){
        if(_firstRevealed.id == _secondRevealed.id){
            _score++;
            _turns++;
            scoreLabel.text = "Score: " + _score;
            turnsLabel.text = "Turns: " + _turns;
        }else{
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
            _turns++;
            turnsLabel.text = "Turns: " + _turns;
        }
        _firstRevealed = null;
        _secondRevealed = null;
    }
    void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = {0,0,1,1,2,2,3,3,4,4};
        numbers = ShuffleArray(numbers);
        for(int i = 0; i < gridCols; i++){
            for(int j = 0; j < gridRows; j++){
                MemoryCard card;
                if(i == 0 && j == 0){
                    card = originalCard;
                }else{
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX,posY, startPos.z);
                
            }
        }
        
    }
    private int[] ShuffleArray(int[] numbers){
        int [] newArray = numbers.Clone() as int[];
        for(int i = 0; i < newArray.Length; i++){
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] =  newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
    public void Restart(){
        Application.LoadLevel("Memory");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
