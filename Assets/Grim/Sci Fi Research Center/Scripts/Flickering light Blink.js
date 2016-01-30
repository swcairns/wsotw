var flickering : boolean = true;
 
function Start() {
        flickerLight();
}
 
function Update () {
 
}
 
function flickerLight() {
 
        while(flickering) {
                yield WaitForSeconds(0.2);
                gameObject.GetComponent.<Light>().intensity += 2;
                yield WaitForSeconds(0.2);
                gameObject.GetComponent.<Light>().intensity = 0;
        }
}