// OLD CODE SNIPPETS FROM WOBBROCK'S SOFTWARE


// Old velocity...

				/************ START: Old Velocity Analyzer ********************
                
                dx = shoePos.X - lastX;
                dy = shoePos.Y - lastY;
                dt = TimeEx.NowMs - lastTime;
                //Console.WriteLine(curTime.ToString());

                // 2D trial
                if(_tdata.Circular)
                {
                    distance = Math.Sqrt((Math.Pow(_tdata.TargetCenter.X - shoePos.X, 2) + Math.Pow(_tdata.TargetCenter.Y - shoePos.Y, 2)));
                    velocity = Math.Sqrt(Math.Pow(dx/dt,2) + Math.Pow(dy/dt, 2));
                }
                // 1D trial (only x-direction movement)
                else
                {
                    distance = Math.Abs(_tdata.TargetCenter.X - shoePos.X);
                    velocity = Math.Abs(dx/dt);
                    //Console.WriteLine(distance.ToString() + "\t" + velocity.ToString());
                }

                ************ END: Old Velocity Analyzer ********************/
				
				
// Debugger for Math.Round
			/*
            for (int j = 11; j<160; j++)
            {
                int factor = (int)Math.Round((j / 10.0f));
                Console.WriteLine(j.ToString() + ": " + factor.ToString());
            }
            */
			
			
			
// Debugger for Buffer.BlockCopy

			/*
            for(int j=0; j<xVelocityBuffer.Length; j++)
            {
                Console.WriteLine("Factor = " + j.ToString());

                for (int i=0; i<xVelocityBuffer.Length; i++)
                {
                    xVelocityBuffer[i] = i;
                    Console.Write(xVelocityBuffer[i].ToString() + ", ");
                }

                Console.WriteLine();

                Buffer.BlockCopy(xVelocityBuffer, 0, xVelocityBuffer, 4*j, 4*(16-j) );       // Buffer.BlockCopy(src, srcOffset, dest, destOffset, totalDataSize);

                for (int i = 1; i<j; i++)
                {
                    xVelocityBuffer[i] = xVelocityBuffer[0];
                }

                for (int i = 0; i < xVelocityBuffer.Length; i++)
                {
                    Console.Write(xVelocityBuffer[i].ToString() + ", ");
                }
                
                Console.WriteLine();
                Console.WriteLine();
            }
             * */
