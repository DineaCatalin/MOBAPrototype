using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    // Queue that contains all the commands that come from the server
    static Queue<Command> commandQueue;

    // Start is called before the first frame update
    void Start()
    {
        commandQueue = new Queue<Command>();
    }

    // Process as many commands as you can in the Fixed Timestep of the FixedUpdate
    void FixedUpdate()
    {
        float startTime = Time.deltaTime;

        while(Time.deltaTime - startTime < Time.fixedDeltaTime)
        {
            ProcessCommand();
        }
    }

    // Add a command to the queue, this sould be called from the outside
    public void AddCommand(Command command)
    {
        commandQueue.Enqueue(command);
    }

    // 
    void ProcessCommand()
    {
        Command command = commandQueue.Dequeue();
        command.execute();
    }
}
