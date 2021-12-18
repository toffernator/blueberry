const findClosestScrollContainer = (element) => {
  while (element) {
    const style = getComputedStyle(element);
    if (style.overflowY !== "visible") {
      return element;
    }
    element = element.parentElement;
  }
  return null;
};

export const initialize = (lastItemIndicator, componentInstance) => {
  const options = {
    root: findClosestScrollContainer(lastItemIndicator),
    rootMargin: "0px",
    threshold: 0,
  };

  const observer = new IntersectionObserver(async (entries) => {
    entries.forEach(async (entry) => {
      if (entry.isIntersecting) {
        await componentInstance.invokeMethodAsync("LoadMoreItems");
      }
    });
  }, options);

  observer.observe(lastItemIndicator);

  return {
    dispose: () => observer.disconnect(),
    handleNewItems: () => {
      observer.unobserve(lastItemIndicator);
      observer.observe(lastItemIndicator);
    },
  };
};
