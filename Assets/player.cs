using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//TODO: change to touch !




public class player : Singleton<player>
{
    // Start is called before the first frame update
    /* UI */

    public List<Image> hp_units ;
    public List<Sprite> hp_icons;
    
    // weapon
    public List<Image> weapon_icon; 
    public List<TextMeshProUGUI> ammo_num;

    public List<WeaponType> weapon_types;

    public List<WeaponBase> weapons;
    
    // weapon
   

    public Sprite grey_panel;
    public Transform hp_area;
    private int max_hp_count ;
    public int hp_count ; 

   
    public TextMeshProUGUI gear_num;



    [HideInInspector] public GunWeapon machinGun;
    [HideInInspector] public GunWeapon shotGun;

    [HideInInspector] public GunWeapon sniper ;
    int sniper_pos = -1 ;

     /* UI */
    /* reward */
    public int gearNum;

    /* reward */

    /* weapon */

    int empty_pos = 0;
   
    /* weapon */

    /* show */
    public GameObject hand;
    public Transform weapon_pos;

    public GameObject ammo ;
    public GameObject target_sprite;
    /* show */

    public Dictionary< WeaponType, WeaponBase > weaponRecords;
    
    // skill
    public playerSkill playerSkills;
    public playerSkill_v2 playerSkills_v2;
    // good
    public shopGood shopgood;
    public bool canFire = true;
    bool canHit = true;
    float firingCoolDown = 0f;

    int shotNum = 1;
    int spread = 1;
        
    int weapon_num;

    int nextEquipWeaponPos;
    protected Material material;
    protected Material hand_material;
    protected Color tint_color;    

    void updateHpUI(){
        
        int r = hp_count % 3; 
        int q = hp_count / 3;

        for(int i = 0 ; i < hp_units.Count ; i++ ){
            if( i < r){
                hp_units[i].sprite = hp_icons[q+1];
            }
            else{
                hp_units[i].sprite = hp_icons[q];
            }   
        }
       
         
    }

    public void GearUpdate(int num){
        gearNum -= num;
        updateGearUI();
    }

    public void updateGearUI(){
        gear_num.text = gearNum.ToString();
    }

    public void updateWeaponUI(){
        Utils.Instance.SetTimer( ()=>{updateAmmoUI();}, 0.01f);
        
        //weapon_pos
    }

    public void updateAmmoUI(){
        GunWeapon gun_weapon = (GunWeapon)(weapons[0]);   
        

       
        if(gun_weapon.gun_type == WeaponType.pistol){
            ammo_num[0].text  = "∞";
        }
        else{
            ammo_num[0].text = gun_weapon.currentAmmoNum.ToString();
        }

        for(int i = 1 ; i <weapon_num;i++){
            GunWeapon sub = (GunWeapon)(weapons[i]);
            //print(sub_1.currentAmmoNum);
            if(sub == null || sub.gun_type == WeaponType.none){
                ammo_num[i].text = "";
            }
            else{
                if( sub.gun_type == WeaponType.pistol){
                    ammo_num[i].text  = "∞";
                }
                else{

                    ammo_num[i].text = sub.currentAmmoNum.ToString();
                }
           
            }
        }


        
        
        
    }


    public void switchWeaponCur(int In){
       
        switchWeapon(In);    
    }

    public void switchWeapon(int In , int Out = 0){
        if(weapons[In] == null){
            return;
        }

        if(In == sniper_pos){
            sniper_pos = Out;
        }
        else if(Out == sniper_pos){
            sniper_pos = In ;
        }

        WeaponType tmp1 = weapon_types[In];
        weapon_types[In] = weapon_types[Out];
        weapon_types[Out] = tmp1;

        WeaponBase tmp2 = weapons[In];
        weapons[In] = weapons[Out];
        weapons[Out] = tmp2 ;

        setWeaponSprite(In);
        setWeaponSprite(Out);
        //ammo update in weapon update !
        updateWeaponUI();
    }
    new private void Awake() {
        base.Awake();
        playerSkills_v2 = new playerSkill_v2();
        playerSkills_v2.OnSkillUnlocked_v2 += PlayerSkills_OnSkillUnlocked_v2;

        //shopgood = new shopGood(playerSkills);
        //shopgood.OnGoodBuy += Player_OnGoodBuy;

        if(GetComponent<SpriteRenderer>() != null ){
            
            material = GetComponent<SpriteRenderer>().material;
            hand_material = hand.GetComponent<SpriteRenderer>().material;
            tint_color = material.GetColor("_tint");
            tint_color.a = 0 ;
        }
    }
    void Start()
    {   
        //weapon1 = WeaponType.machineGun ;
        //weapon2 = WeaponType.shotGun ;
        
        //weaponRecords = helper.DeepCloneDictionary< WeaponType, WeaponBase >(gameDataRecorder.Instance.weaponRecords);
        //weapons = helper.DeepCloneList<WeaponBase>( gameDataRecorder.Instance.weapons);
        //gearNum = gameDataRecorder.Instance.gear;
        //weapon_types = helper.DeepCloneList<WeaponType>(gameDataRecorder.Instance.weaponTypes);
        weapon_types = new List<WeaponType>(){WeaponType.pistol , WeaponType.none, WeaponType.none, WeaponType.none,WeaponType.none};
        weaponRecords = new Dictionary<WeaponType, WeaponBase>();
        weapons = new List<WeaponBase>(); 
        gearNum = 0;

        weapon_num = weapon_icon.Count;
        nextEquipWeaponPos = 1;
        
        for(int i = 0 ; i < weapon_num ; i ++){
            if(weapon_types[i] == WeaponType.none){
                weapons.Add(null);
                continue;
            }
            ScriptableWeaponBase scriptable =  ResourceSystem.Instance.GetWeaponData(weapon_types[i]);
            WeaponBase new_weapon = Instantiate(scriptable.prefab, transform.position + -100*Vector3.right, Quaternion.identity);
            new_weapon.transform.SetParent(transform);
            new_weapon.SetData(weapon_types[i]);
            weaponRecords[weapon_types[i]] = new_weapon;
            weapons.Add(new_weapon);
        }


        for(int i = 0 ; i < weapon_num ; i++){
            setWeaponSprite(i);
        }
        


        Utils.Instance.SetTimer(
            ()=>{
                updateAmmoUI();
                updateHpUI();
                updateGearUI(); 
            } , 0.01f
        );

        
        machinGun = (GunWeapon)createWeapon(WeaponType.machineGun,nextEquipWeaponPos);
        equipWeapon(WeaponType.machineGun,nextEquipWeaponPos);
        nextEquipWeaponPos+=1;
             
        
        max_hp_count = 3;

        // cheatUp(playerSkill.SkillType.penetrate_1);
        // cheatUp(playerSkill.SkillType.penetrate_2);
        // cheatUp(playerSkill.SkillType.sniper);
        // cheatUp(playerSkill.SkillType.atk_up);
    }
    // private void Player_OnGoodBuy(object sender, shopGood.OnGoodBuyEventArgs e){
    //     switch(e.goodType){
    //         case shopGood._GoodType.allAmmo10:
    //             for(int i = 0 ; i < weapon_num ; i++ ){
    //                 if( weapon_types[i] != WeaponType.none && weapon_types[i] != WeaponType.pistol ){
    //                     GunWeapon tmp = (GunWeapon)weaponRecords[weapon_types[i]];
    //                     equipAmmo(weapon_types[i], tmp.gunArgs.maxAmmoAmount /10 );
    //                 }
    //             }
    //             break;
    //         case shopGood._GoodType.hp_up:
    //             if( max_hp_count > hp_count){
    //                 hp_count += 1;
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.machineGunAmmo:
    //             if(equipAmmo(WeaponType.machineGun, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.shotGunAmmo:
    //             if(equipAmmo(WeaponType.shotGun, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.laserAmmo:
    //             if(equipAmmo(WeaponType.laser, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.bombAmmo:
    //             if(equipAmmo(WeaponType.cannon, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.rocketAmmo:
    //             if(equipAmmo(WeaponType.rocket, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //         case shopGood._GoodType.sniperAmmo:
    //             if(equipAmmo(WeaponType.snipper, 10)){
    //                 gearNum -= e.price;
    //             }
    //             break;
    //     }
        
    //     updateWeaponUI();
    //     updateAmmoUI();
    //     updateHpUI();    
    //     updateGearUI();
    // }

   
     private void PlayerSkills_OnSkillUnlocked_v2(object sender, playerSkill_v2.OnSkillUnlockedEventArgs_v2 e) {
        switch (e.skillType) {
            case playerSkill_v2.SkillType.machineGun_v1:
                machinGun = (GunWeapon)createWeapon(WeaponType.machineGun,nextEquipWeaponPos);
                equipWeapon(WeaponType.machineGun,nextEquipWeaponPos);
                nextEquipWeaponPos+=1;
                break;
            
            case playerSkill_v2.SkillType.machineGun_v2:
                machinGun.baseArgs.coolDown -= 5;
                break;

            case playerSkill_v2.SkillType.machineGun_v3:
                machinGun.baseArgs.coolDown -= 5;
                break;

            case playerSkill_v2.SkillType.shotGun_v1:
                shotGun = (GunWeapon)createWeapon(WeaponType.shotGun,nextEquipWeaponPos);
                equipWeapon(WeaponType.shotGun,nextEquipWeaponPos);
                nextEquipWeaponPos+=1;
                break;
            
            case playerSkill_v2.SkillType.shotGun_v2:
                shotGun.gunArgs.NumOfShotgunAmmo += 4;
                break;
            
            case playerSkill_v2.SkillType.shotGun_v3:
                shotGun.gunArgs.shotgunAngle += 10;
                shotGun.gunArgs.NumOfShotgunAmmo += 4;
                break;
            
            case playerSkill_v2.SkillType.sniper_v1:
                sniper =  (GunWeapon)createWeapon(WeaponType.snipper,nextEquipWeaponPos);
                equipWeapon(WeaponType.snipper,nextEquipWeaponPos);
                sniper_pos = nextEquipWeaponPos;
                nextEquipWeaponPos+=1;
                break;

            case playerSkill_v2.SkillType.sniper_v2:
                sniper.ammoLevelUp();
                break;

            case playerSkill_v2.SkillType.sniper_v3:
                sniper =  (GunWeapon)createWeapon(WeaponType.laser,sniper_pos);
                equipWeapon(WeaponType.laser,sniper_pos);
                break;

        }
    }



    // private void PlayerSkills_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e) {
    //     switch (e.skillType) {
    //         case playerSkill.SkillType.max_hp_2:
    //             max_hp_count += 3;
    //             hp_count = max_hp_count; 
    //             updateHpUI();
    //             break;
    //         case playerSkill.SkillType.max_hp_3:
    //             max_hp_count += 3;
    //             hp_count = max_hp_count; 
    //             updateHpUI();
    //             break;
    //         case playerSkill.SkillType.spread_2:
    //             spread +=1;
    //             GunWeapon gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.isShotgun = true;
    //             gun_weapon.gunArgs.shotgunAngle = 30f;
    //             gun_weapon.gunArgs.NumOfShotgunAmmo = spread;
    //             break;
    //         case playerSkill.SkillType.spread_3:
    //             spread +=1;
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.NumOfShotgunAmmo = spread;
    //             break;    
    //         case playerSkill.SkillType.doubleShot:
    //             shotNum += 1;
    //             break;
    //         case playerSkill.SkillType.tripleShot:
    //             shotNum += 1;
    //             break;
    //         case playerSkill.SkillType.atk_up:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.atk += 1;
    //             break;

            
    //         case playerSkill.SkillType.small_explosion:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.atk += 1;
    //             gun_weapon.gunArgs.small_explode = true;
    //             break;

    //         case playerSkill.SkillType.penetrate_1:
    //             gun_weapon =(GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.penerateNum += 1;
    //             break;
    //         case playerSkill.SkillType.penetrate_2:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.penerateNum += 1;
    //             break;


    //         case playerSkill.SkillType.machineGun:

    //             createWeapon(WeaponType.machineGun,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.machineGun,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.sniper:
    //             createWeapon(WeaponType.snipper,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.snipper,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.laser:
    //             createWeapon(WeaponType.laser,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.laser,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.shotGun:
    //             createWeapon(WeaponType.shotGun,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.shotGun,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.bomb:
    //             createWeapon(WeaponType.cannon,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.cannon,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.rocket:
    //             createWeapon(WeaponType.rocket,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.rocket,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.flamer_thrower:
    //             createWeapon(WeaponType.flame_thrower,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.flame_thrower,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;

    //     }
    // }
    
    void setWeaponSprite( int n ){
        if(weapon_types[n] == WeaponType.none){
            return ;
        }
        //print(ResourceSystem.Instance);
        ScriptableWeaponBase scriptable =  ResourceSystem.Instance.GetWeaponData(weapon_types[n]);
        weapon_icon[n].sprite= scriptable.sprite;
        
        if(n == 0){
           if( weapon_pos.childCount != 0){
                Transform old_weapon = weapon_pos.GetChild(0);
                old_weapon.SetParent(gameDataRecorder.Instance.transform);
                old_weapon.gameObject.SetActive(false);
           }
           
           //old_weapon.position = 
           weapons[n].gameObject.SetActive(true);
          
           weapons[n].transform.SetParent(weapon_pos);
           weapons[n].transform.position = weapon_pos.position;
           weapons[n].transform.rotation = hand.transform.rotation * Quaternion.AngleAxis( 18f , Vector3.forward);
        }

    }
    public void endLevel(){
        weapons[0].transform.SetParent(gameDataRecorder.Instance.transform);
        weapons[0].gameObject.SetActive(false);
    }
    public WeaponBase createWeapon( WeaponType w , int number = -1 ){
        if(w ==  WeaponType.none){
            return null;
        }
  
        ScriptableWeaponBase scriptable =  ResourceSystem.Instance.GetWeaponData(w);
        WeaponBase new_weapon = Instantiate(scriptable.prefab, weapon_pos.position , Quaternion.identity);
        
        new_weapon.SetData(w);
        new_weapon.gameObject.SetActive(false);
        //update icon 
        if(number != -1){
            setWeaponSprite(number);
        }

        new_weapon.transform.SetParent(gameDataRecorder.Instance.transform);
        
        //new_weapon

        weaponRecords[w] = new_weapon;
        return new_weapon;
    }

    public WeaponBase equipWeapon( WeaponType w , int number){
        
        weapon_types[number] = w;
        if(w == WeaponType.none){
        
            weapon_icon[number].sprite = grey_panel;
            weapons[number] = null;
            ammo_num[number].text = "";
            return null;
        }
  

        if( weaponRecords.ContainsKey(w) ){
            weapons[number] = weaponRecords[w];
        }  
        else{
            weapons[number] = createWeapon( w  , number );
        }
   
        setWeaponSprite(number);
        updateWeaponUI();

        return weapons[number];
    }

  
    public bool equipAmmo( WeaponType w , int num){
        GunWeapon targetWeapon;
        if( weaponRecords.ContainsKey(w) ){
            targetWeapon = (GunWeapon)weaponRecords[w];
        }  
        else{
            print("bug at equipAmmo in player.cs") ;
            targetWeapon = null;
        }

        bool res = targetWeapon.getAmmo(num);
        updateAmmoUI();
        return res;
//        shop_manager.Instance.UpdateAmmoInstorehouse(w, targetWeapon.currentAmmoNum);
    }

    void hpIncrease(){
        if( hp_count < max_hp_count){
            hp_count+=1;
            updateHpUI();
        }
    }
    void hpDncrease(){
        tint_color.a = 1;
        StartCoroutine(Itint());
        if( hp_count > 0){
            hp_count-=1;
            updateHpUI();
        }
        if(hp_count == 0){
            level_manager.Instance.levelFailed();
        }
    }

    public void getReward( RewardType t , int num ){

        gearNum += num;
        updateGearUI();
    }

    // Update is called once per frame

    IEnumerator Ifire(GunWeapon weapon){
        for(int i = 0 ; i < shotNum ; i++){
            GameObject new_ammo = Instantiate( weapon.getAmmo() );   
                
            
            new_ammo.GetComponent<Ammo_Template>().SetArgs( weapon.gunArgs, (GunWeapon)weapons[0] );
            new_ammo.transform.eulerAngles = transform.eulerAngles;
            weapon.fire();//equipWeapon
            new_ammo.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    IEnumerator Itint(){
        float interval = 2f;
        int flash_time = 4;
        for(int i = 0 ; i < flash_time ; i ++){
            tint_color.a = 1;
            while(tint_color.a > 0){

                tint_color.a -= (1f/( (interval * 100f )/flash_time));
                yield return new WaitForSeconds(0.01f); 
            }
        }
        

    }
    void Update()
    {       
    

        
        
        material.SetColor("_tint" , tint_color  );  
        hand_material.SetColor("_tint" , tint_color  );  
        
        if( helper.IsOverUI() ){
            // do nothing
        }
        
        else if(weapons[0].GetComponent<GunWeapon>()!= null &&
            Input.GetMouseButton(0) && canFire){
            
            
            //get mouse Pos
         
            Vector2 mousePos ;
            mousePos.x  = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            mousePos.y  = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            
            // set aimer sprite
            target_sprite.SetActive(true);
            firingCoolDown = weapons[0].baseArgs.coolDown/100f  ;
               
            target_sprite.transform.position = mousePos;

            Vector2 relative = new Vector2(  mousePos.x - transform.position.x , mousePos.y - transform.position.y ); 
            
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

            Vector2 weaponRelative = new Vector2(  mousePos.x - transform.position.x , mousePos.y - transform.position.y ); 


      
            hand.transform.eulerAngles = new Vector3(0,0,-18f);
            hand.transform.RotateAround( transform.position, Vector3.forward,  angle);
     
            GunWeapon weapon = (GunWeapon)weapons[0];
            if(weapon.currentAmmoNum <= 0 ){
                
                //Out of ammo

            }
       
            else{
                
                
                if(weapon.gunArgs.isRapid){
                    StartCoroutine(Ifire(weapon));
                }
                else{
                    
                  


                    GameObject new_ammo = Instantiate( weapon.getAmmo() );   
                    new_ammo.GetComponent<Ammo_Template>().SetArgs( weapon.gunArgs, (GunWeapon)weapons[0] );
         
                    new_ammo.transform.eulerAngles = transform.eulerAngles;
                    weapon.fire();//equipWeapon

                    new_ammo.gameObject.SetActive(true);
                }

                bool isAllZero = true;
                for(int i = 0 ; i < 3 ; i ++){
                    GunWeapon tmp = (GunWeapon)(weapons[i]);
                    if( tmp.currentAmmoNum != 0 ){
                        isAllZero = false;
                        break;
                    } 
                }
                if(isAllZero){
                    equipWeapon(WeaponType.pistol,0);
                }
            



                
                
                canFire = false;
                Utils.Instance.SetTimer( ()=>{canFire = true;}, weapons[0].baseArgs.coolDown /100f);
                //update ammo num ui 
                updateAmmoUI();
            }
        }
      
        else if(Input.GetMouseButton(0)){
            
            
            Vector2 mousePos ;
            mousePos.x  = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            mousePos.y  = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            
             // set aimer sprite
            target_sprite.SetActive(true);
            firingCoolDown = weapons[0].baseArgs.coolDown/100f  ;
               
            target_sprite.transform.position = mousePos;

            Vector2 relative = new Vector2(  mousePos.x - transform.position.x , mousePos.y - transform.position.y ); 
          
     
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

            hand.transform.eulerAngles = new Vector3(0,0,-18f);
            hand.transform.RotateAround( transform.position, Vector3.forward,  angle);
      
        }
        
        if(firingCoolDown > 0){
           
            firingCoolDown -= Time.deltaTime;
           
        }
        else{
            
            target_sprite.SetActive(false);
        }


        // if(Input.GetMouseButtonDown(1) ){
        //     equipWeapon(weapon_types[1],1);
        // }

        // testing
        if( Input.GetKeyDown(KeyCode.S) ){
            hpIncrease();
        }
        if( Input.GetKeyDown(KeyCode.A) ){
            hpDncrease();
        }

        if( Input.GetKeyDown(KeyCode.Q) ){
            shop_panel.Instance.open();
        }

     
    }


   
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.GetComponent<EnemyBase>()!=null){
            other.gameObject.GetComponent<EnemyBase>()._Destroy(false);
            if( canHit){
                canHit = false;
                hpDncrease();
                Utils.Instance.SetTimer( ()=>{canHit = true;}, 2f);
            }
        }
    }


    //  private void cheatUp(playerSkill.SkillType t){
    //     switch (t) {
    //         case playerSkill.SkillType.max_hp_2:
    //             max_hp_count += 3;
    //             hp_count = max_hp_count; 
    //             updateHpUI();
    //             break;
    //         case playerSkill.SkillType.max_hp_3:
    //             max_hp_count += 3;
    //             hp_count = max_hp_count; 
    //             updateHpUI();
    //             break;
    //         case playerSkill.SkillType.spread_2:
    //             spread +=1;
    //             GunWeapon gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.isShotgun = true;
    //             gun_weapon.gunArgs.shotgunAngle = 30f;
    //             gun_weapon.gunArgs.NumOfShotgunAmmo = spread;
    //             break;
    //         case playerSkill.SkillType.spread_3:
    //             spread +=1;
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.NumOfShotgunAmmo = spread;
    //             break;    
    //         case playerSkill.SkillType.doubleShot:
    //             shotNum += 1;
    //             break;
    //         case playerSkill.SkillType.tripleShot:
    //             shotNum += 1;
    //             break;
    //         case playerSkill.SkillType.atk_up:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.atk += 1;
    //             break;

            
    //         case playerSkill.SkillType.small_explosion:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.atk += 1;
    //             gun_weapon.gunArgs.small_explode = true;
    //             break;

    //         case playerSkill.SkillType.penetrate_1:
    //             gun_weapon =(GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.penerateNum += 1;
    //             break;
    //         case playerSkill.SkillType.penetrate_2:
    //             gun_weapon = (GunWeapon)weaponRecords[WeaponType.pistol];
    //             gun_weapon.gunArgs.penerateNum += 1;
    //             break;


    //         case playerSkill.SkillType.machineGun:

    //             createWeapon(WeaponType.machineGun,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.machineGun,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.sniper:
    //             createWeapon(WeaponType.snipper,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.snipper,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.laser:
    //             createWeapon(WeaponType.laser,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.laser,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.shotGun:
    //             createWeapon(WeaponType.shotGun,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.shotGun,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.bomb:
    //             createWeapon(WeaponType.cannon,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.cannon,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.rocket:
    //             createWeapon(WeaponType.rocket,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.rocket,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;
    //         case playerSkill.SkillType.flamer_thrower:
    //             createWeapon(WeaponType.flame_thrower,nextEquipWeaponPos);
    //             equipWeapon(WeaponType.flame_thrower,nextEquipWeaponPos);
    //             nextEquipWeaponPos+=1;
    //             break;

    //     }
    // }
}
