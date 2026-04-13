# Classroom-Anomaly
- Uses Unity 6

## DEV SCENE COMMITS:

- Make prefabs out of your stuff when possible (e.g., UI is a prefab, each NPC is a prefab, the Student's VR Rig is a prefab, etc).
- When making pre-fabs:
    - In **your scene**, set up your stuff **exactly** where it needs to be in the game world, with the **exact** configuration necessary, in the **exact** way it is intended to be used in the final product.
    - Drag your GameObjects from the *Hierarchy* window into the your assets folder in the **Project** window.

### Individual Scene/Branch changes:

1. Preparing the commits:
    1. With the editor still open, ensure your changes are complete and ready to be committed.
    2. Save all in the editor and CLOSE THE EDITOR.
    3. Fetch all, from your branch and `dev`.
    4. Consistently check if pushes to `dev` have been made (switch to `dev` and fetch all). If so, **merge `dev` into your branch**. You won't be able to make a PR until this occurs, but making the PR should require you to be up-to-date with dev.
        - You want to make sure that your branch is up-to-date with everyone's latest changes!
    5. If your modified pre-fab(s) is(are) already in `dev` scene, skip the next step.
    6. If your new/modified pre-fab(s) has(have) **not** been placed in `dev` scene:
      - Ensure you're on *YOUR* branch. Since we're using PRs, we longer have to make `dev` scene changes on the `dev` branch. We aren't even able to anymore, and this is a good thing since a PR will package the changes to your scene *and* the `dev` scene all in one reviewable unit.
      - Re-open the Unity project, then navigate to the project-wide "Classroom" scene in the Assets->Scenes folder. Open it.
      - Having created your pre-fabs, drag your pre-fabs from the Project Assets window into the *HIERARCHY* window in this scene. Do NOT drag them into the SCENE window, since they will not be in the same place as when you created the pre-fab. After dragging them into the Hierarchy they shouldn't need any configuration or modification (since they're pre-fabs) but if they do, configure them.
      - Save all in the editor on this scene.
      - **Switch back to your scene**. Switching back here ensures you don't accidentally modify the shared scene next time you open the project.
      - Save all again, just to be safe.
      - CLOSE THE EDITOR.
    7. Commit to your branch. Do not push.
      - In the case of merge conflicts, kept changes should come from your branch and scene. If there's a conflict with the files, most of them should come from your own assets. If there's a conflict with the `dev` scene, we may need to get the team involved.
    8. Push to your branch!
2. Read and complete the steps in the section [Making A PR](#making-a-pr). 
3. Wait for your PR to be merged (or merge it yourself if necessary).
4. You're done! Keep working!

### Making A PR

If pull requests (PRs) are unfamiliar to you, all you need to know is that they are a formal, organized way to merge one branch into another. You are "requesting" that the changes on your branch be "pulled" into the project, and it wraps your commits into a container with a title and a description. When you're ready to make something official, you make a PR to let everyone know it's ready, and (ideally) someone else approves your changes. Ours is configured to allow us to merge our own, but in most cases they should be merged by someone else to ensure they have a second set of eyes.

In our case, the `dev` and `main` branches are protected by requiring a pull request in order to merge into them. These branches also cannot be modified directly anymore - this is why you now modify the `dev` scene on your branch. Doing it this way slows things down a bit, but PRs will automatically send notifications to everyone when published and they allow comments and code reviews before merging. Additionally, once a PR is made, more commits to that branch will automatically be added to the PR if it has not been merged yet.

You can also make a draft PR if you'd like other people to see your commits before the entire PR is ready. Draft PRs cannot be merged until their status is updated. We don't need to worry about draft PRs too much, but they're useful for collaborative efforts or when feedback is needed before finishing. 

Lastly, when a PR is merged, it will often recommend that the merging branch be deleted. We do not want to do this. Do not click anything saying it will delete the branch. We use these! You will still be able to push to your branch after merging a PR.

Here's how to use a PR for our project:
1. In GitHub desktop, you should have already pushed your changes to **your** branch. If you've made your commits but you haven't pushed, **push**.
2. Click the button that says "Preview Pull Request" and submit a descriptive PR for merging into `dev` that notes what you've done and any issues with your changes or feedback you need. It's important that your notes are descriptive, since Unity files are notoriously hard to read and we want other people to easily understand your work when they review it.
    - If you cannot make or edit your PR in GitHub desktop, you can submit or modify a submitted PR on the GitHub page for this repository.
4. In the case of merge conflicts:
    - If there are merge conflicts, keep the version from **your branch**. Only files you've just modified should ever be problematic. If there's a problem with the `dev` scene that you cannot figure out on your own, get in touch with the group.
    - If you aren't sure what to do, message the @Marcia role on Discord.
5. Let everyone know that you've made a PR (even though they should be notified anyway) and ask for someone to review it.
6. PRs do not have to be merged immediately (unless the changes are needed urgently) but if nobody can review it make sure the group is okay with you merging it yourself, and go to to the repo page -> Pull Requests and merge it! Then pull `dev` on your machine and you're good to go! Just make sure to switch to your branch before getting back to work.

