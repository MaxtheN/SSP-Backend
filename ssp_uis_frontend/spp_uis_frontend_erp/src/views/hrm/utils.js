// eslint-disable-next-line import/prefer-default-export
export const sortHrmOrder = (list = []) => {
    const sortedData = list.sort((a, b) => {
        if (a.isHR && !b.isHR) return -1;
        if (!a.isHR && b.isHR) return 1;
        if (a.isDirector && !b.isDirector) return 1;
        if (!a.isDirector && b.isDirector) return -1;
        return a.signOrder - b.signOrder;
    });

    // Update signOrder based on the sorted order
    sortedData.forEach((item, index) => {
        item.signOrder = index + 1;
    });


    return sortedData

}