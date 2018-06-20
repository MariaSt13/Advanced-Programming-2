package com.example.linoycohen.imageservice;

import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Environment;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.NotificationManagerCompat;
import android.util.Log;
import android.widget.Toast;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class ImageServiceService extends Service {

    BroadcastReceiver myReceiver;
    OutputStream output;
    InputStream input;

    public ImageServiceService(){}
    @Nullable
    @Override


    public IBinder onBind(Intent intent)
    {
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
    }

    public int onStartCommand(Intent intent, int flag, int startId)
    {
        Toast.makeText(this,"Service starting...", Toast.LENGTH_SHORT).show();
        final IntentFilter theFilter = new IntentFilter();
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");

        this.myReceiver = new BroadcastReceiver() {
            boolean flag =  false;
            @Override
            public void onReceive(Context context, Intent intent) {
                WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);
                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {
                        //get the different network states
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {
                            if(flag == false) {
                                flag = true;
                                Thread thread = new Thread() {
                                    Socket socket;
                                    public void run() {
                                        try {
                                            //here you must put your computer's IP address
                                            InetAddress serverAddr = InetAddress.getByName("10.0.2.2");
                                            //create a socket to make the connection with the server
                                            socket = new Socket(serverAddr, 8000);
                                            output = socket.getOutputStream();
                                            input = socket.getInputStream();
                                            //Starting the Transfer
                                            startTransfer();
                                        } catch (Exception e) {
                                            Log.e("TCP", "C: Error", e);
                                        } finally {
                                            flag = false;
                                            try {
                                                socket.close();
                                                flag = false;
                                            } catch (IOException e) {
                                                e.printStackTrace();
                                            }
                                        }
                                        }

                                };
                                thread.start();
                            }
                        }
                    }
                }
            }
        };
        // Registers the receiver so that your service will listen for broadcasts
         this.registerReceiver(this.myReceiver, theFilter);
         return START_STICKY;
    }

    public void startTransfer() {
        // Getting the Camera Folder

        //NotificationManagerCompat notificationManager = NotificationManagerCompat.from(this);
      //  NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "default");
        File dcim = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM + "/Camera");
        if (dcim == null) {
            return;
        }
        File[] pics = dcim.listFiles();
        int count = 0;
        if (pics != null) {
         //   builder.setContentTitle("Picture Transfer").setContentText("Transfer in progress").setPriority(NotificationCompat.PRIORITY_LOW);
            for (File pic : pics) {
                try {
           //         builder.setContentText("Half way through").setProgress(100, 50, false);
             //       notificationManager.notify(1, builder.build());
                    FileInputStream fis = new FileInputStream(pic);
                    Bitmap bm = BitmapFactory.decodeStream(fis);
                    byte[] imgbyte = getBytesFromBitmap(bm);
                    //sends the message to the server
                    output.write(imgbyte);

                    int i;
                    char c;
                    byte[] str = new byte[7];
                    // reads till the end of the stream
                    while((i = input.read(str))!=-1) {

                        //if(i == 7){
                            break;
                        //}

                    }

                } catch (Exception e) {
                    Log.e("TCP", "S: Error", e);
                }
            }
            // At the End
            //builder.setContentText("Download complete").setProgress(0, 0, false);
            //notificationManager.notify(1, builder.build());
        }
    }

    public byte[] getBytesFromBitmap(Bitmap bitmap){
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }

    public void onDestroy() {
        Toast.makeText(this,"Service ending...", Toast.LENGTH_SHORT).show();
    }
}
