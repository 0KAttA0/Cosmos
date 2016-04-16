using UnityEngine;
using System.Collections;

public class SoundSculptures : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

#if 0
/*
 * /**
 * MOUSE              : rotate view --> durch Geste ersetzen

 * KEYS
 * 1                  : Aufnahem und Speicherung Audio 1
 * 2                  : Aufnahem und Speicherung Audio 1
 * 3                  : Abspielen Audio 1
 * 4                  : Abspielen Audio 2
 */


// ------ imports ------

import processing.pdf.*;
import java.util.Calendar;
import ddf.minim.*;


// ------ initial parameters and declarations ------

int pointCount = 500;
PVector[] lissajousPoints = new PVector[0];
// LIste um punkte zu speichern = wavedatei
//ArrayList pointList = new ArrayList();

float x, y, z;
float d, a, h;
float zpos = 0;

float lineWeight = 1;
color lineColor = 255;
//color lineColor = color(0, 130, 164);
float lineAlpha = 50;

float connectionRadius = 100;
float connectionRamp = 6;

// ------ mouse interaction ------
int offsetX = 0, offsetY = 0, clickX = 0, clickY = 0, zoom=-400;
float rotationX = 0, rotationY = 0, targetRotationX = 0, targetRotationY = 0, clickRotationX, clickRotationY; 

// ----- audio input --------
Minim minim;
AudioInput input;


void setup() {
	size(displayWidth, displayHeight, P3D);
	smooth();
	colorMode(RGB, 255, 255, 255, 100);
	background(0);
	lights();
	strokeWeight(lineWeight);
	strokeCap(ROUND);
	noFill();
	//frameRate(2);

	minim = new Minim(this);
	input = minim.getLineIn (Minim.STEREO, 512);
}


void draw() {
	// background(0);
	// ------ mouse interaction ------> LEAP
	if (mousePressed && mouseButton==RIGHT) {
		offsetX = mouseX-clickX;
		offsetY = mouseY-clickY;
		targetRotationX = min(max(clickRotationX + offsetY/float(width) * TWO_PI, -HALF_PI), HALF_PI);
		targetRotationY = clickRotationY + offsetX/float(height) * TWO_PI;
	}
	rotationX += (targetRotationX-rotationX)*0.25; 
	rotationY += (targetRotationY-rotationY)*0.25;  
	rotateX(-rotationX);
	rotateY(rotationY); 

	// Auslesen und speichern des Spektrums
	//float[] buffer = input.mix.toArray ();

	// Berechnung der Lissajous-Figur auf x-, y- und Z-Achse
	lissajousPoints = new PVector[pointCount+1];

	for (int i=0; i<=pointCount; i++) {
		float angle = map(i, 0, pointCount, 0,TWO_PI);

		float freqY = input.mix.level()*10;

		float x = (sin(angle * 2));
		// float x = sin(angle * freqX + radians(phi)) * cos (angle * modFreqX)
		float y = sin(angle * freqY)* cos (angle * 14) ;
		float z = (sin(angle * 5) + radians (30));

		x = x * z *(width/ 2 -50);
		y = y *(height / 2 - 50);
		zpos = zpos + 0.0005;

		// berechneter Punkt wird gespeichert
		lissajousPoints[i] = new PVector(x, y, zpos);  

	}

	pushMatrix();
	translate(width/2, height/2, zpos); 
	/**for (float xpos = 0; xpos < width; xpos++){
    xpos = xpos + 0.01;
   }
    if (xpos > width){
      xpos = 0;
    }
    */

	// beide for-Schleifen erwirken, dass jeder Punkt mit jedem verbunden wird --> var i1 durchläuft alle Punkte
	// var i2 durchläuft Punkte von index 0 bis index i1-1 --> vermeidet, dass Linien doppelt gezeichnet werden
	for (int i1 = 0; i1 < pointCount; i1++) {
		for (int i2 = 0; i2 < i1; i2++) {
			PVector p1 = lissajousPoints[i1];
			PVector p2 = lissajousPoints[i2];

			d = PVector.dist(p1, p2);
			// Distanz zw. beiden Punkten wird bestimmt --> daruas Transparenzwert a berechnet (Wert zw. 0 und 1) 
			a = pow(1/(d/connectionRadius+1), 6);

			// Linie wird nur gezeichnet, wenn Distanz kleiner als festgelgter Radius
			if (d <= connectionRadius) {
				// --> Maximalwert für Deckkraft lineAplha multipliziert --> Ergibt Wert zw. 0 und lineAplpha
				stroke(lineColor,  a*lineAlpha);
				line(p1.x, p1.y, p1.z, p2.x, p2.y, p2.z);

			} 
		}
	}

	popMatrix();

}


void mousePressed(){
	clickX = mouseX;
	clickY = mouseY;
	clickRotationX = rotationX;
	clickRotationY = rotationY;
}

#endif