using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MouseTools
{
    public class CustomCursor : MonoBehaviour
    {
        public Animator animControl;

        public Sprite baseOuterCircle;
        public Sprite baseInnerCircle;
        public Sprite overGreenBot;
        public Sprite overBlueBot;
        public Sprite overOrangeBot;
        public Sprite overLeadBot;
        public Sprite overEnemyBot;

        public bool leadBot;
        public bool blueBot;
        public bool greenBot;
        public bool orangeBot;
        public bool enemyBot;

        public AIMachine selectedBot;
        public GameObject targetEnemy;

        public Image outerImage;
        public Image innerImage;

        private bool clickHeld = false;
        public bool overUI = false;
        public bool overBot = false;
        public bool overEnemy = false;
        public bool botSelected = false;

        private void Start()
        {
            Cursor.visible = false;
            animControl = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localPoint);
            transform.localPosition = localPoint;

            if (Input.GetKeyDown(KeyCode.Mouse0) && clickHeld == false)
            {
                PlayAnim("MouseClick");
                clickHeld = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && clickHeld == true)
            {
                PlayAnim("MouseClickRelease");
                clickHeld = false;
            }

            if (overUI)
                animControl.Play("MouseEnterUI");

            if (!overUI)
                animControl.Play("MouseExitUI");

            if(!overBot && !overEnemy && botSelected && Input.GetKeyDown(KeyCode.Mouse1))
            {
                ResetValues();
            }
        }

        public void ResetValues()
        {
            Debug.Log("resetting mouse values");
            botSelected = false;
            leadBot = false;
            blueBot = false;
            greenBot = false;
            orangeBot = false;
            enemyBot = false;
            selectedBot = null;
            ChangeCursor(5);
        }

        public void PlayAnim(string animName) //used to play any of the animations that are on the mouse's controller
        {
            animControl.Play(animName);
        }

        public void SetAnimBool(string boolName, bool value)
        {
            animControl.SetBool(boolName, value);
        }

        public void AnimationEnded()
        {
            SetAnimBool("Reloading", false);
        }

        public void ChangeCursor(int cursorID) //used to change the cursor based on an ID value
        {
            if (cursorID == 0)
                innerImage.sprite = overGreenBot;
            else if (cursorID == 1)
                innerImage.sprite = overBlueBot;
            else if (cursorID == 2)
                innerImage.sprite = overOrangeBot;
            else if (cursorID == 3)
                innerImage.sprite = overLeadBot;
            else if (cursorID == 4)
                innerImage.sprite = overEnemyBot;
            else if (cursorID == 5)
            {
                innerImage.sprite = baseInnerCircle;
                outerImage.sprite = baseOuterCircle;
            }
        }
    }
}

