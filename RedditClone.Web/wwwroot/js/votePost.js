$(async function () {
    const upVoteButtonSelectedColorClass = "selected-up-vote";
    const downVoteButtonSelectedColorClass = "selected-down-vote";
    const defaultVoteButtonColorClass = "link-grey";
    const addUpVoteUrl = "Identity/VotePost/UpVote";
    const removeUpVoteUrl = "Identity/VotePost/RemoveUpVote";
    const addDownVoteUrl = "Identity/VotePost/DownVote";
    const removeDownVoteUrl = "Identity/VotePost/RemoveDownVote";

    let upVoteButtons = $(".upVote");
    let downVoteButtons = $(".downVote");

    await attachUpVoteEvent(upVoteButtons);
    await attachDownVoteEvent(downVoteButtons);

    async function attachUpVoteEvent(elements) {
        $(elements).on("click", async function (event) {
            event.preventDefault();

            let clickedUpVoteBtn = $(event.currentTarget);
            let postId = clickedUpVoteBtn.attr("data-postId");
            let voteValueDiv = getVoteValueDivFromPostId(postId);
            let isUpBtnSelected = isButtonAlreadySelected(clickedUpVoteBtn);
            if (isUpBtnSelected) {
                let isRequestSuccessful = makeVotePostPostRequest(postId, removeUpVoteUrl);
                if (isRequestSuccessful) {
                    descreaseVoteValue(voteValueDiv, 1);
                    changeUpVoteButtonColorToDefault(clickedUpVoteBtn);
                }
            }
            else {
                let isRequestSuccessful = await makeVotePostPostRequest(postId, addUpVoteUrl);
                if (isRequestSuccessful) {
                    let increaseCount = 0;

                    let downVoteButton = getClosestDownVoteButtonFromUpVoteButton(clickedUpVoteBtn);
                    let isDownBtnSelected = isButtonAlreadySelected(downVoteButton);
                    if (isDownBtnSelected) {
                        changeDownVoteButtonColorToDefault(downVoteButton);
                        increaseCount = 2;
                    }
                    else {
                        increaseCount = 1;
                    }

                    increaseVoteValue(voteValueDiv, increaseCount);
                    changeUpVoteButtonColorToSelected(clickedUpVoteBtn);
                }
            }
        })
    }

    async function attachDownVoteEvent(elements) {
        $(elements).on("click", async function (event) {
            event.preventDefault();

            let clickedDownVoteBtn = $(event.currentTarget);
            let postId = clickedDownVoteBtn.attr("data-postId");
            let voteValueDiv = getVoteValueDivFromPostId(postId);
            let isBtnSelected = isButtonAlreadySelected(clickedDownVoteBtn);
            if (isBtnSelected) {
                let isRequestSuccessful = makeVotePostPostRequest(postId, removeDownVoteUrl);
                if (isRequestSuccessful) {
                    increaseVoteValue(voteValueDiv, 1);
                    changeDownVoteButtonColorToDefault(clickedDownVoteBtn);
                }
            }
            else {
                let isRequestSuccessful = await makeVotePostPostRequest(postId, addDownVoteUrl);
                if (isRequestSuccessful) {
                    let increaseCount = 0;

                    let upVoteButton = getClosestUpVoteButtonFromDownVoteButton(clickedDownVoteBtn);
                    let isUpBtnSelected = isButtonAlreadySelected(upVoteButton);
                    if (isUpBtnSelected) {
                        changeUpVoteButtonColorToDefault(upVoteButton);
                        increaseCount = 2;
                    }
                    else {
                        increaseCount = 1;
                    }

                    descreaseVoteValue(voteValueDiv, increaseCount);
                    changeDownVoteButtonColorToSelected(clickedDownVoteBtn);
                }
            }
        })
    }

    function isButtonAlreadySelected(btn) {
        if ($(btn).hasClass(upVoteButtonSelectedColorClass) || $(btn).hasClass(downVoteButtonSelectedColorClass)) {
            return true;
        }
        else {
            return false;
        }
    }

    async function makeVotePostPostRequest(postId, url) {
        let isRequestSuccessful = false;

        let antiForgeryToken = $('input:hidden[name="__RequestVerificationToken"]').val();
        await $.ajax({
            url: url,
            data: {
                postId: postId
            },
            type: "POST",
            headers: {
                RequestVerificationToken: antiForgeryToken
            },
            success: function () {
                isRequestSuccessful = true;
            }
        })

        return isRequestSuccessful;
    }

    function getVoteValueDivFromPostId(postId) {
        let voteValueDiv = $(`div.vote-value[data-postId='${postId}']`)
        return voteValueDiv;
    }

    function descreaseVoteValue(voteValueDiv, count) {
        let voteValueInt = parseInt($(voteValueDiv).text());
        $(voteValueDiv).text(voteValueInt - count);
    }

    function increaseVoteValue(voteValueDiv, count) {
        let voteValueInt = parseInt($(voteValueDiv).text());
        $(voteValueDiv).text(voteValueInt + count);
    }

    function getClosestDownVoteButtonFromUpVoteButton(upVoteButton) {
        let upVoteSiblings = upVoteButton.parent().siblings("div");
        let downVoteBtn = upVoteSiblings.find(".downVote")[0];

        return downVoteBtn;
    }

    function getClosestUpVoteButtonFromDownVoteButton(downVoteButton) {
        let downVoteSiblings = downVoteButton.parent().siblings("div");
        let upVoteBtn = downVoteSiblings.find(".upVote")[0];

        return upVoteBtn;
    }

    function changeUpVoteButtonColorToSelected(upVoteButton) {
        removeDefaultArrowColor(upVoteButton);
        addUpVoteSpecificColor(upVoteButton);
    }

    function addUpVoteSpecificColor(element) {
        $(element).addClass(upVoteButtonSelectedColorClass);
    }

    function changeDownVoteButtonColorToDefault(downVoteButton) {
        removeDownVoteSpecificColor(downVoteButton);
        addDefaultArrowColor(downVoteButton);
    }

    function removeDownVoteSpecificColor(element) {
        $(element).removeClass(downVoteButtonSelectedColorClass);
    }

    function changeDownVoteButtonColorToSelected(downVoteButton) {
        removeDefaultArrowColor(downVoteButton);
        addDownVoteSpecificColor(downVoteButton);
    }

    function removeDefaultArrowColor(element) {
        $(element).removeClass(defaultVoteButtonColorClass);
    }

    function addDownVoteSpecificColor(element) {
        $(element).addClass(downVoteButtonSelectedColorClass);
    }

    function changeUpVoteButtonColorToDefault(upVoteButton) {
        removeUpVoteSpecificColor(upVoteButton);
        addDefaultArrowColor(upVoteButton);
    }

    function removeUpVoteSpecificColor(upVoteButton) {
        $(upVoteButton).removeClass(upVoteButtonSelectedColorClass);
    }

    function addDefaultArrowColor(element) {
        $(element).addClass(defaultVoteButtonColorClass);
    }
})