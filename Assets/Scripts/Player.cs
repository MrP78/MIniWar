using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    MoveChecker moveChecker;
    ParticleSystem ps;
    CharacterAtributes atri;
    InformationGatherer gather;
    [SerializeField]
    Slider healthBar;

    public AI ai;
    public bool isAttacking = false;
    public bool turn = false;
    public Player[] players;
    public bool IsMoving = false;
    public bool AttackUsed = false;
    public float speed = 2f;
    public bool done = false;
    public int moves;
    Animator anim;
    private Canvas canvas;
    private Dropdown menu;
    private int orginalMoves;
    TurnOver turnover;
    public bool IsSelected = false;
    Menu ActionMenu;
    GameSpace gameSpace;

    List<string> DropOptions1 = new List<string> { "Choose Action", "Move", "Attack" };
    List<string> DropOptions2 = new List<string> { "Choose Action", "", "Attack" };
    List<string> DropOptions3 = new List<string> { "Choose Action", "Move", "" };
    List<string> DropOptions4 = new List<string> { "No More Actions", "", "" };
   
    void Start ()
    {
        ai = GameObject.FindObjectOfType<AI>();
        anim = GetComponent<Animator>();
        turnover = GameObject.FindObjectOfType<TurnOver>();
        atri = GetComponent<CharacterAtributes>();
        gather = GameObject.FindObjectOfType<InformationGatherer>();
        moveChecker = GameObject.FindObjectOfType<MoveChecker>();
        healthBar.maxValue = atri.health;
        healthBar.value = atri.health;
        ActionMenu = GameObject.FindObjectOfType<Menu>();
        canvas= GameObject.Find("ActionMenu").GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        menu = GameObject.Find("ActionMenu").GetComponentInChildren<Dropdown>();
        menu.ClearOptions();
        menu.AddOptions(DropOptions1);
        menu.Hide();        
        orginalMoves = moves;
        ps = GetComponent<ParticleSystem>();
        gameSpace = GameObject.FindObjectOfType<GameSpace>();

       
    }	
	
	void Update () {        
        if (IsSelected == false)
        {
            var em = ps.emission;
            em.enabled = false;
        }
        if (IsSelected)
        {
           menu.Show();
            var em = ps.emission;
            em.enabled = true;
        }       
    }    

    private void OnMouseDown()
    {
        if (turn)
        {
            ActiveTurnActions();
        }
        if (!turn)
        {
            InactiveTurnActions();
        }
    }

    public void ActiveTurnActions()
    {
        ActionMenu.SetPlayer(this);
        var em = ps.emission;
        em.enabled = true;
        ByeMenu();
        DrawMenu();
        menu.Show();
        DestroySquares();
        players = GameObject.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.IsSelected = false;
        }
        IsSelected = true;        
    }

    public void InactiveTurnActions()
    {
        Player SelectedPlayer = ActionMenu.GetPlayer();
        
        if (SelectedPlayer !=null && SelectedPlayer.isAttacking)
        {           
            gather.GetTargetInfo(this.gameObject, this.transform.position, atri.Armour);            
            ActionMenu.Attack();
        }
    }

    public void DrawMenu()
    {
        menu.Hide();
        if (moves < 1 && !AttackUsed)
        {
            menu.ClearOptions();
            menu.AddOptions(DropOptions2);

        }
        if (moves > 1 && AttackUsed )
        {
            menu.ClearOptions();
            menu.AddOptions(DropOptions3);
        }
        if (moves < 1 && AttackUsed)
        {
            menu.ClearOptions();
            menu.AddOptions(DropOptions4);

        }
        if (moves > 0 && !AttackUsed)
        {
            menu.ClearOptions();
            menu.AddOptions(DropOptions1);
        }
        menu.Show();
    } 
    
    public void Move()
    {
        //Vector2 movedFrom = new Vector2
        gameSpace.roundedPos = new Vector2(this.transform.position.x, this.transform.position.y);
        menu.value = -1;
        isAttacking = false;
        gather.ClearInfo();
        players = GameObject.FindObjectsOfType<Player>();
        if (moves > 0)
        {
            foreach (Player player in players)
            {
                player.IsMoving = false;
            }
            this.IsMoving = true;
            moveChecker.CheckAvaliable(this.transform.position,moves);
            moveChecker.ShowSquares();
            
        }        
    }

    public void Attack()
    {
        menu.value = -1;
        moveChecker.ClearFree();
        moveChecker.CheckAvaliable(this.transform.position, 1);              
       
        int R = CheckRange();
        
        if(gather.selectedTargetPos.x < this.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3( -1,transform.localScale.y);
        }
        if(gather.selectedTargetPos.x > this.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y);
        }

        if(moveChecker.IsOccupied(gather.selectedTargetPos.x,gather.selectedTargetPos.y))
        {            
            anim.SetTrigger("attackTrigger");
        }
        else if(atri.shooting > 0 && atri.range >= R)
        {            
            anim.SetTrigger("rangedAttackTrigger");
        }
        else {
            turnover.SetMessage(("Target out of range"),0.75f);
            done = true;           
        }        
    }

    public void MeleeAttack()
    {
        AttackUsed = true;
        int hitRoll = Random.Range(1, 11);
        //print("hitRoll " + hitRoll);
        if (hitRoll + atri.strength >= 8)
        {
            turnover.SetMessage(("Hit!"),0.50f);
            DrawMenu();
            MeleeDamage();
            moveChecker.ClearFree();
        }
        else
        {
            turnover.SetMessage(("Miss!"), 0.75f);
            AttackUsed = true;
            DrawMenu();
            isAttacking = false;
            moveChecker.ClearFree();
            done = true;
            return;
        }        
    }

    public void RangedAttack()
    {
        AttackUsed = true;
        int hitRoll = Random.Range(1, 11);
        //print("hitRoll " + hitRoll);
        if (hitRoll + atri.shooting >= 8)
        {
            turnover.SetMessage(("Hit!"), 0.50f);            
            RangedDamage();
            DrawMenu();
            //moveChecker.ClearFree();
        }
        else
        {
            turnover.SetMessage(("Miss!"), 0.75f);
            AttackUsed = true;            
            DrawMenu();
            isAttacking = false;
            moveChecker.ClearFree();
            done = true;
            return;
        }       
    }

    public void MeleeDamage()
    {
        int damRoll = Random.Range(1, 7);
        //print("damRoll " + damRoll);
        if(atri.meleeDamage + damRoll > gather.selectedTargetArm)
        {
            int damage = (atri.meleeDamage + damRoll)-(gather.selectedTargetArm);
            StartCoroutine(DamageSetter(damage + " Damage", 0.75f));
            gather.DoDamage(-damage);
            done = true;
            isAttacking = false;
        }
        else
        {
            StartCoroutine(DamageSetter("No damage", 0.75f));
            done = true;
            isAttacking = false;
        }

    }

    public void RangedDamage()
    {
        int damRoll = Random.Range(1, 7);
        //print("damRoll " + damRoll);
        if (atri.rangedDamage + damRoll > gather.selectedTargetArm)
        {
            int damage = (atri.rangedDamage + damRoll)-(gather.selectedTargetArm) ;
            StartCoroutine(DamageSetter(damage + " Damage", 0.75f));            
            gather.DoDamage(-damage);
            done = true;
            isAttacking = false;            
        }
        else
        {
            StartCoroutine(DamageSetter("No damage",0.75f));
            isAttacking = false;
            done = true;
        }
    }

    public void ResetMoves()
    {
        moves = orginalMoves;
        AttackUsed = false;
        isAttacking = false;
        menu.ClearOptions();
        menu.AddOptions(DropOptions1);
        IsSelected = false;
        menu.Hide();
        DestroySquares();
        menu.value = -1;
        done = false;
        ai.j = 0;
        ai.currentPlayer = null;
        moveChecker.ClearFree();
    }

    public void ByeMenu()
    {
        menu.Hide();
    }
    public void DestroySquares()
    {
        moveChecker.DestroySquares();
    }
    public void ChangeHealth(int dam)
    {
        atri.health += dam;
        healthBar.value = atri.health;
        if(atri.health <= 0)
        {
            StartCoroutine("Dead");
        }
    }

    IEnumerator DamageSetter(string message, float n) {
        yield return new WaitForSeconds(0.5f);
        turnover.SetMessage(message, n);
    }

    public void SetAniBool(string b, bool l)
    {
        anim.SetBool(b,l);
    }

    int CheckRange()
    {       
        int n1 = Mathf.Abs((int)transform.position.x - (int)gather.selectedTarget.transform.position.x);
        int n2 = Mathf.Abs((int)transform.position.y - (int)gather.selectedTarget.transform.position.y);
        int m = n1 + n2;
        //print("range"+m);
        return m;
    }

    IEnumerator Dead()
    {
        anim.SetTrigger("dieTrigger");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

    }
}
